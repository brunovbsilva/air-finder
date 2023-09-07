using AirFinder.Application.Games.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.Games.Models.Responses;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Create a new game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request) { 
            return Response(await _gameService.CreateGame(request, GetUserId(HttpContext))); 
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Returns a list of games in pages of 15 games")]
        [ProducesResponseType(typeof(ListGamesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListGames([FromQuery] ListGamesRequest request) { 
            return Response(await _gameService.ListGames(request, GetUserId(HttpContext))); 
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Returns a game by id")]
        [ProducesResponseType(typeof(GetDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id) { 
            return Response(await _gameService.GetDetails(id)); 
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update a game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGame([FromBody] UpdateGameRequest request) { 
            return Response(await _gameService.UpdateGame(request, GetUserId(HttpContext))); 
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteGame([FromRoute] Guid id) { 
            return Response(await _gameService.DeleteGame(id, GetUserId(HttpContext))); 
        }

        [HttpPost("join/{id}")]
        [SwaggerOperation(Summary = "Join a game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> JoinGame([FromRoute] Guid id) { 
            return Response(await _gameService.JoinGame(id, GetUserId(HttpContext))); 
        }

        [HttpDelete("leave/{id}")]
        [SwaggerOperation(Summary = "Leave a game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LeaveGame([FromRoute] Guid id) { 
            return Response(await _gameService.LeaveGame(id, GetUserId(HttpContext))); 
        }
        
        [HttpPost("pay/{id}")]
        [SwaggerOperation(Summary = "Pay a game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PayGame([FromRoute] Guid id) { 
            return Response(await _gameService.PayGame(id, GetUserId(HttpContext))); 
        }

        [HttpPost("validate")]
        [SwaggerOperation(Summary = "Validate if user already payed the game")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> ValidateGameJoin([FromBody] ValidateGameJoinRequest request) { 
            return Response(await _gameService.ValidateGameJoin(request, GetUserId(HttpContext))); 
        }
    }
}
