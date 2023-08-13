using AirFinder.Application.Battlegrounds.Services;
using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Battlegrounds.Models.Responses;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AirFinder.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BattlegroundController : BaseController
    {
        private readonly IBattlegroundService _battlegroundService;
        public BattlegroundController(INotification notification, IBattlegroundService battlegroundService) : base(notification)
        {
            _battlegroundService = battlegroundService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all battlegrounds registered by the user")]
        [ProducesResponseType(typeof(GetBattlegroundsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBattlegrounds()
        {
            return Response(await _battlegroundService.GetBattlegrounds(GetUserId(HttpContext)));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new battleground")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBattleground([FromBody] CreateBattlegroundRequest request)
        {
            return Response(await _battlegroundService.CreateBattleground(GetUserId(HttpContext), request));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a battleground")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBattleground(Guid id)
        {
            return Response(await _battlegroundService.DeleteBattleground(id));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a battleground")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBattleground([FromRoute] Guid id, [FromBody] UpdateBattlegroundRequest request)
        {
            return Response(await _battlegroundService.UpdateBattleground(id, request));
        }
    }
}
