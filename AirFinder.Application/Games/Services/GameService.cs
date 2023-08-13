using AirFinder.Application.Email.Services;
using AirFinder.Domain.BattleGrounds;
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
        readonly IBattleGroundRepository _battleGroundRepository;
        readonly IGameLogRepository _gameLogRepository;
        public GameService(
            INotification notification,
            IMailService mailService,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IBattleGroundRepository battleGroundRepository,
            IGameLogRepository gameLogRepository
        ) : base(notification, mailService) 
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _battleGroundRepository = battleGroundRepository;
            _gameLogRepository = gameLogRepository;
        }

        public async Task<BaseResponse?> CreateGame(CreateGameRequest request, Guid userId) => await ExecuteAsync(async () => 
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            var bg = await _battleGroundRepository.GetByIDAsync(request.IdBattleground) ?? throw new NotFoundBattlegroundException();
            await _gameRepository.InsertWithSaveChangesAsync(new Game(request, userId));
            return new GenericResponse();
        });
        public async Task<BaseResponse?> UpdateGame(UpdateGameRequest request, Guid userId) => await ExecuteAsync(async () => 
        {
            var game = await _gameRepository.GetByIDAsync(request.Id) ?? throw new NotFoundGameException();
            if (game.IdCreator != userId) throw new MethodNotAllowedException();

            game.Update(request);

            await _gameRepository.UpdateWithSaveChangesAsync(game);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> DeleteGame(Guid id, Guid userId) => await ExecuteAsync(async () => 
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            var game = await _gameRepository.GetByIDAsync(id) ?? throw new NotFoundGameException();
            if (game.IdCreator != user.Id) throw new MethodNotAllowedException();
            await _gameRepository.DeleteAsync(id);
            return new GenericResponse();
        });
        public async Task<ListGamesResponse?> ListGames(ListGamesRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            return await _gameRepository.getGameList(request, userId);
        });
        public async Task<GetDetailsResponse?> GetDetails(Guid id) => await ExecuteAsync(async () => {
            var game = await _gameRepository.GetByIDAsync(id) ?? throw new NotFoundGameException();
            return new GetDetailsResponse{ Game = (GameDto)game }; 
        });
        public async Task<BaseResponse?> JoinGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => 
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new NotFoundGameException();
            if (game.IdCreator == user.Id) throw new MethodNotAllowedException();
            var gameLog = new GameLog(gameId, userId);
            await _gameLogRepository.InsertWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> LeaveGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => 
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new NotFoundGameException();
            if (game.IdCreator == user.Id) throw new MethodNotAllowedException();
            var gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == game.Id && x.UserId == user.Id).FirstOrDefaultAsync() ?? throw new NotFoundGameLogException();
            
            await _gameLogRepository.DeleteAsync(gameLog);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> PayGame(Guid gameId, Guid userId) => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new NotFoundGameException();
            var gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == gameId && x.UserId == userId).FirstOrDefaultAsync() ?? throw new NotFoundGameLogException();
            
            gameLog.PaymentDate = DateTime.Now.Ticks;
            await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> ValidateGameJoin(ValidateGameJoinRequest request, Guid userId) => await ExecuteAsync(async () => 
        {
            var user = await _userRepository.GetByIDAsync(request.UserId) ?? throw new NotFoundUserException();
            var game = await _gameRepository.GetByIDAsync(request.GameId) ?? throw new NotFoundGameException();
            if (game.IdCreator != userId) throw new MethodNotAllowedException();
            var gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == request.GameId && x.UserId == request.UserId).FirstOrDefaultAsync() ?? throw new NotFoundGameLogException();
            if (gameLog.PaymentDate == null) throw new MethodNotAllowedException();

            gameLog.ValidateDate = DateTime.Now.Ticks;
            await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });

    }
}