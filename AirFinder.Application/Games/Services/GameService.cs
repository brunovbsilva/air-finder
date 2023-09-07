using AirFinder.Application.Email.Services;
using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Common;
using AirFinder.Domain.GameLogs;
using AirFinder.Domain.Games;
using AirFinder.Domain.Games.Models.Dtos;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.Games.Models.Responses;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Application.Games.Services
{
    public class GameService : BaseService, IGameService
    {
        readonly IUserRepository _userRepository;
        readonly IGameRepository _gameRepository;
        readonly IBattlegroundRepository _battlegroundRepository;
        readonly IGameLogRepository _gameLogRepository;
        public GameService(
            INotification notification,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IBattlegroundRepository battlegroundRepository,
            IGameLogRepository gameLogRepository
        ) : base(notification) 
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _battlegroundRepository = battlegroundRepository;
            _gameLogRepository = gameLogRepository;
        }

        #region CreateGame
        public async Task<BaseResponse> CreateGame(CreateGameRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            if(!await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            if(!await _battlegroundRepository.AnyAsync(x => x.Id == request.IdBattleground && x.IdCreator == userId)) throw new NotFoundBattlegroundException();
            await _gameRepository.InsertWithSaveChangesAsync(new Game(request, userId));
            return new GenericResponse();
        });
        #endregion

        #region ListGames
        public async Task<ListGamesResponse> ListGames(ListGamesRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            if (await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            return await _gameRepository.getGameList(request, userId);
        });
        #endregion

        #region GetDetails
        public async Task<GetDetailsResponse> GetDetails(Guid id) => await ExecuteAsync(async () => {
            var game = await _gameRepository.GetByIDAsync(id) ?? throw new NotFoundGameException();
            return new GetDetailsResponse { Game = (GameDto)game };
        });
        #endregion

        #region UpdateGame
        public async Task<BaseResponse> UpdateGame(UpdateGameRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            var game = await _gameRepository.Get(x => x.Id == request.Id && x.IdCreator == userId).FirstOrDefaultAsync() ?? throw new NotFoundGameException();

            game.Update(request);

            await _gameRepository.UpdateWithSaveChangesAsync(game);
            return new GenericResponse();
        });
        #endregion

        #region DeleteGame
        public async Task<BaseResponse> DeleteGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => 
        {
            if (await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            if (await _gameRepository.AnyAsync(x => x.Id == gameId && x.IdCreator == userId)) throw new NotFoundGameException();
            await _gameRepository.DeleteAsync(gameId);
            return new GenericResponse();
        });
        #endregion

        #region JoinGame
        public async Task<BaseResponse> JoinGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => 
        {
            if (await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            if (await _gameRepository.AnyAsync(x => x.Id == gameId && x.IdCreator == userId)) throw new NotFoundGameException();
            var gameLog = new GameLog(gameId, userId);
            await _gameLogRepository.InsertWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });
        #endregion

        #region LeaveGame
        public async Task<BaseResponse> LeaveGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => 
        {
            if (await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            if (await _gameRepository.AnyAsync(x => x.Id == gameId && x.IdCreator == userId)) throw new NotFoundGameException();
            var gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == gameId && x.UserId == userId).FirstOrDefaultAsync() ?? throw new NotFoundGameLogException();
            
            await _gameLogRepository.DeleteAsync(gameLog);
            return new GenericResponse();
        });
        #endregion

        #region PayGame
        public async Task<BaseResponse> PayGame(Guid gameId, Guid userId) => await ExecuteAsync(async () =>
        {
            if (await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            if (await _gameRepository.AnyAsync(x => x.Id == gameId && x.IdCreator == userId)) throw new NotFoundGameException();
            var gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == gameId && x.UserId == userId).FirstOrDefaultAsync() ?? throw new NotFoundGameLogException();
            
            gameLog.PaymentDate = DateTime.Now.Ticks;
            await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });
        #endregion

        #region ValidateGameJoin
        public async Task<BaseResponse> ValidateGameJoin(ValidateGameJoinRequest request, Guid userId) => await ExecuteAsync(async () => 
        {
            if (await _userRepository.AnyAsync(x => x.Id == userId)) throw new NotFoundUserException();
            if (await _gameRepository.AnyAsync(x => x.Id == request.GameId && x.IdCreator == userId)) throw new NotFoundGameException();
            var gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == request.GameId && x.UserId == request.UserId).FirstOrDefaultAsync() ?? throw new NotFoundGameLogException();
            if (gameLog.PaymentDate == null) throw new MethodNotAllowedException();

            gameLog.ValidateDate = DateTime.Now.Ticks;
            await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });
        #endregion

    }
}