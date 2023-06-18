using AirFinder.Application.Email.Services;
using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.Common;
using AirFinder.Domain.GameLogs;
using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games;
using AirFinder.Domain.Games.Models.Dtos;
using AirFinder.Domain.Games.Models.Enums;
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
            var game = new Game(request.Name, request.Description, request.Date, request.IdBattleground, userId);
            await _gameRepository.InsertWithSaveChangesAsync(game);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> UpdateGame(UpdateGameRequest request, Guid userId) => await ExecuteAsync(async () => {
            var game = await _gameRepository.GetByIDAsync(request.Id) ?? throw new ArgumentException("Game not found");
            if (game.IdCreator != userId) throw new MethodNotAllowedException();

            game.Name = request.Name;
            game.Description = request.Description;
            game.MillisDate = request.Date;

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
        public async Task<GetDetailsResponse?> GetDetails(Guid id) => await ExecuteAsync(async () => {
            var game = await _gameRepository.GetByIDAsync(id) ?? throw new ArgumentException("Game not found");
            return new GetDetailsResponse{ Game = (GameDto)game }; 
        });
        public async Task<BaseResponse?> JoinGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new ArgumentException("Game not found");
            if (game.Status != GameStatus.Created) throw new ArgumentException("You can not join this game now");
            var log = await _gameLogRepository.GetAll().Where(x => x.GameId == gameId && x.UserId == userId).FirstOrDefaultAsync();
            if (log != null) throw new ArgumentException("User already joined this game");
            log = new GameLog(gameId, userId, DateTime.Now.Ticks, "JoinGame");
            await _gameLogRepository.InsertWithSaveChangesAsync(log);
            return new GenericResponse();
        });
        public async Task<ListGamesResponse?> ListGames(ListGamesRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            var response = new ListGamesResponse();
            List<GameLogStatus>? joinStatusList = null;
            List<GameStatus>? gameStatusList = null;
            if(request.JoinStatusList != null) joinStatusList = request.JoinStatusList.Split(',').Select(x => (GameLogStatus)Enum.Parse(typeof(GameLogStatus), x)).ToList();
            if(request.GameStatusList != null) gameStatusList = request.GameStatusList.Split(',').Select(x => (GameStatus)Enum.Parse(typeof(GameStatus), x)).ToList();
            response = await _gameRepository.getGameList(request.PageIndex, request.ItemsPerPage, joinStatusList, gameStatusList, request.FromDate, request.UpToDate, userId);
            return response;
        });
        public async Task<BaseResponse?> ValidateGameJoin(ValidateGameJoinRequest request, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var gameLog = await _gameLogRepository.GetByIDAsync(request.IdGameLog) ?? throw new ArgumentException("Log not found");

            if (gameLog.UserId != request.UserId) throw new MethodNotAllowedException();
            if (gameLog.Status != GameLogStatus.Joined) throw new ArgumentException("Not paied yet");
            if (gameLog.Status == GameLogStatus.Validated) throw new ArgumentException("QRCode already validated");
            if (gameLog.Status == GameLogStatus.Finished) throw new ArgumentException("Game finished");

            gameLog.Status = GameLogStatus.Validated;
            gameLog.LastUpdateDate = DateTime.Now.Ticks;
            gameLog.LastUpdateUserId = userId;

            await _gameLogRepository.UpdateWithSaveChangesAsync(gameLog);
            return new GenericResponse(); 
        });
        public async Task<BaseResponse?> LeaveGame(Guid gameId, Guid userId) => await ExecuteAsync(async () => {
            var user = await _userRepository.GetByIDAsync(userId) ?? throw new ArgumentException("User not found");
            var game = await _gameRepository.GetByIDAsync(gameId) ?? throw new ArgumentException("Game not found");
            if (game.Status != GameStatus.Created) throw new ArgumentException("You can not leave this game now");
            var log = await _gameLogRepository.GetAll().Where(x => x.GameId == gameId && x.UserId == userId).FirstOrDefaultAsync();
            if (log == null) throw new ArgumentException("User do not joined this game");
            await _gameLogRepository.DeleteAsync(log);
            return new GenericResponse();
        });
        public async Task<BaseResponse?> PayGame(PayGameRequest request, Guid userId) => await ExecuteAsync(async () =>
        {
            throw new NotImplementedException();
            return new GenericResponse();
        });
    }
}