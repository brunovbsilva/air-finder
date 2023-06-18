using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.GameLogs;
using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games;
using AirFinder.Domain.Games.Models.Dtos;
using AirFinder.Domain.Games.Models.Enums;
using AirFinder.Domain.Games.Models.Responses;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AirFinder.Infra.Data.Repository
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public GameRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ListGamesResponse> getGameList(int currentPage, int itemsPerPage, List<GameLogStatus>? joinStatusList, List<GameStatus>? gameStatusList, long? fromDate, long? upToDate, Guid userId)
        {
            var tbGame = _unitOfWork.Context.Set<Game>().AsNoTracking();
            var tbBG = _unitOfWork.Context.Set<BattleGround>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();
            var tbLogs = _unitOfWork.Context.Set<GameLog>().AsNoTracking();

            var query = (
                from g in tbGame
                join bg in tbBG on g.IdBattleGround equals bg.Id into BGs from bgd in BGs.DefaultIfEmpty()
                join u in tbUser on g.IdCreator equals u.Id into Us from usd in Us.DefaultIfEmpty()
                join l in tbLogs on g.Id equals l.GameId into Ls from lsd in Ls.DefaultIfEmpty()

                select new GameCardDto()
                {
                    Id = g.Id,
                    CreatorId = g.IdCreator,
                    Name = g.Name,
                    Local = $"{bgd.City}, {bgd.State}",
                    Date = g.MillisDate,
                    ImageUrl = bgd.ImageUrl,
                    Verified = usd.Roll == UserRoll.Admnistrator || usd.Roll == UserRoll.ContentCreator,
                    CanDelete = g.IdCreator == userId,
                    JoinStatus = lsd != null && lsd.UserId == userId ? lsd.Status : GameLogStatus.None,
                    GameStatus = g.Status
                })
                .Where(x =>
                    (x.CreatorId == userId && x.GameStatus == GameStatus.Finished && gameStatusList != null && gameStatusList.Contains(GameStatus.Finished)) ||
                    ((joinStatusList == null || joinStatusList.Contains(x.JoinStatus)) &&
                    (gameStatusList == null || gameStatusList.Contains(x.GameStatus)) &&
                    (fromDate == null || x.Date >= fromDate) &&
                    (upToDate == null || x.Date <= upToDate))
                ).OrderBy(x => x.Date);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            if(currentPage >= totalPages) currentPage = totalPages-1;
            if(currentPage < 0) currentPage = 0;

            var gameList = await query
                .Skip(itemsPerPage * (currentPage))
                .Take(itemsPerPage)
                .ToListAsync();

            return new ListGamesResponse
            {
                Games = gameList,
                Length = totalItems,
                PageIndex = currentPage
            };
        }
    }
}
