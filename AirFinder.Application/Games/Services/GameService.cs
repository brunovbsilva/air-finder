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

        public async Task<BaseResponse?> CreateGame(CreateGameRequest request, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var bg = await _battleGroundRepository.GetByIDAsync(request.IdBattleground) ?? throw new ArgumentException("BattleGround not found");
            var game = new Game(request, userId);
            await _gameRepository.InsertWithSaveChangesAsync(game);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> UpdateGame(UpdateGameRequest request, Guid userId) => await ExecuteAsync(async () => {
            var game = await _gameRepository.GetByIDAsync(request.Id) ?? throw new ArgumentException("Game not found");
            if (game.IdCreator != userId) throw new MethodNotAllowedException();

            game.Name = request.Name;
            game.Description = request.Description;
            game.MillisDateFrom = request.DateFrom;
            game.MillisDateUpTo = request.DateUpTo;
            game.MaxPlayers = request.MaxPlayers ?? 0;

            await _gameRepository.UpdateWithSaveChangesAsync(game);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> DeleteGame(Guid id, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(id) ?? throw new ArgumentException("Game not found");
            if (game.IdCreator != user.Id) throw new MethodNotAllowedException();
            await _gameRepository.DeleteAsync(id);
            return new GenericResponse();
        });
        public async Task<ListGamesResponse?> ListGames(ListGamesRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            return await _gameRepository.getGameList(request, userId);
        });
        public async Task<GetDetailsResponse?> GetDetails(Guid id) => await ExecuteAsync(async () => {
            var game = await _gameRepository.GetByIDAsync(id) ?? throw new ArgumentException("Game not found");
            return new GetDetailsResponse{ Game = (GameDto)game }; 
        });
        public async Task<BaseResponse?> JoinGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => 
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new ArgumentException("Game not found");
            if (game.IdCreator == user.Id) throw new MethodNotAllowedException();
            GameLog gameLog = new GameLog(gameId, userId);
            await _gameLogRepository.InsertWithSaveChangesAsync(gameLog);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> LeaveGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new ArgumentException("Game not found");
            if (game.IdCreator == user.Id) throw new MethodNotAllowedException();
            GameLog? gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == game.Id && x.UserId == user.Id).FirstOrDefaultAsync();
            if (gameLog != null) await _gameLogRepository.DeleteAsync(gameLog);
            else throw new ArgumentException("Log not found");
            return new GenericResponse();
        });
        public async Task<BaseResponse?> PayGame(Guid gameId, Guid userId) => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new ArgumentException("Game not found");
            GameLog? gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == gameId && x.UserId == userId).FirstOrDefaultAsync();
            if(gameLog != null)
            {
                gameLog.PaymentDate = DateTime.Now.Ticks;
                await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            }
            else throw new ArgumentException("Log not found");
            return new GenericResponse();
        });
        public async Task<BaseResponse?> ValidateGameJoin(ValidateGameJoinRequest request, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(request.UserId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(request.GameId) ?? throw new ArgumentException("Game not found");
            if (game.IdCreator != userId) throw new MethodNotAllowedException();
            GameLog? gameLog = await _gameLogRepository.GetAll().Where(x => x.GameId == request.GameId && x.UserId == request.UserId).FirstOrDefaultAsync();
            if (gameLog == null) throw new ArgumentException("Log not found");
            else
            {
                if (gameLog.PaymentDate == null) throw new MethodNotAllowedException();
                gameLog.ValidateDate = DateTime.Now.Ticks;
                await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            }
            return new GenericResponse();
        });
        
    }
}