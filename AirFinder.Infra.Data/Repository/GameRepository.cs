using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.GameLogs;
using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games;
using AirFinder.Domain.Games.Models.Dtos;
using AirFinder.Domain.Games.Models.Enums;
using AirFinder.Domain.Games.Models.Requests;
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

        public async Task<ListGamesResponse> getGameList(ListGamesRequest request, Guid userId)
        {
            var tbGame = _unitOfWork.Context.Set<Game>().AsNoTracking();
            var tbBG = _unitOfWork.Context.Set<BattleGround>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();
            var ticksNow = DateTime.Now.Ticks;

            var query = (
                from g in tbGame
                join bg in tbBG on g.IdBattleGround equals bg.Id into BGs from bgd in BGs.DefaultIfEmpty()
                join u in tbUser on g.IdCreator equals u.Id into Us from usd in Us.DefaultIfEmpty()

                select new GameCardDto()
                {
                    Id = g.Id,
                    CreatorId = g.IdCreator,
                    Name = g.Name,
                    Local = $"{bgd.City}, {bgd.State}",
                    DateFrom = g.MillisDateFrom,
                    DateUpTo = g.MillisDateUpTo,
                    MaxPlayers = g.MaxPlayers,
                    ImageUrl = bgd.ImageUrl,
                    Verified = usd.Roll == UserRoll.Admnistrator || usd.Roll == UserRoll.ContentCreator,
                    CanDelete = g.IdCreator == userId
                })
                .Where(x =>
                    (request.GameStatus == GameStatus.Created && x.DateFrom > ticksNow) ||
                    (request.GameStatus == GameStatus.Started && x.DateFrom < ticksNow && x.DateUpTo > ticksNow) ||
                    (request.GameStatus == GameStatus.Finished && x.DateUpTo < ticksNow) ||
                    (request.GameStatus == GameStatus.All)
                )
                .OrderBy(x => x.DateFrom);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / request.ItemsPerPage);
            if(request.PageIndex >= totalPages) request.PageIndex = totalPages-1;
            if(request.PageIndex < 0) request.PageIndex = 0;

            var gameList = await query
                .Skip(request.ItemsPerPage * (request.PageIndex))
                .Take(request.ItemsPerPage)
                .ToListAsync();

            return new ListGamesResponse
            {
                Games = gameList,
                Length = totalItems,
                PageIndex = request.PageIndex,
            };
        }
    }
}
