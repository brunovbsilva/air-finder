using AirFinder.Application.Games.Services;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirFinder.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GameController : BaseController
    {
        private readonly IGameService _gameService;
        public GameController( INotification notification, IGameService gameService) : base(notification)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request) { return Response(await _gameService.CreateGame(request, GetUserId(HttpContext))); }
        [HttpGet]
        public async Task<IActionResult> ListGames([FromQuery] ListGamesRequest request) { return Response(await _gameService.ListGames(request, GetUserId(HttpContext))); }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id) { return Response(await _gameService.GetDetails(id)); }
        [HttpPut]
        public async Task<IActionResult> UpdateGame([FromBody] UpdateGameRequest request) { return Response(await _gameService.UpdateGame(request, GetUserId(HttpContext))); }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame([FromRoute] Guid id) { return Response(await _gameService.DeleteGame(id, GetUserId(HttpContext))); }
        [HttpPost("join/{id}")]
        public async Task<IActionResult> JoinGame([FromRoute] Guid id) { return Response(await _gameService.JoinGame(id, GetUserId(HttpContext))); }
        [HttpDelete("leave/{id}")]
        public async Task<IActionResult> LeaveGame([FromRoute] Guid id) { return Response(await _gameService.LeaveGame(id, GetUserId(HttpContext))); }
        [HttpPost("pay/{id}")]
        public async Task<IActionResult> PayGame([FromRoute] Guid id) { return Response(await _gameService.PayGame(id, GetUserId(HttpContext))); }
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateGameJoin([FromBody] ValidateGameJoinRequest request) { return Response(await _gameService.ValidateGameJoin(request, GetUserId(HttpContext))); }
    }
}
