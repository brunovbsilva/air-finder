using AirFinder.Application.BattleGrounds.Services;
using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.BattleGrounds.Models.Responses;
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
        private readonly IBattleGroundService _battleGroundService;
        public BattlegroundController(INotification notification, IBattleGroundService battleGroundService) : base(notification)
        {
            _battleGroundService = battleGroundService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all battlegrounds registered by the user")]
        [ProducesResponseType(typeof(GetBattleGroundResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBattlegrounds()
        {
            return Response(await _battleGroundService.GetBattleGrounds(GetUserId(HttpContext)));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new battleground")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBattleground([FromBody] CreateBattleGroundRequest request)
        {
            return Response(await _battleGroundService.CreateBattleGround(GetUserId(HttpContext), request));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a battleground")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBattleground(Guid id)
        {
            return Response(await _battleGroundService.DeleteBattleGround(id));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a battleground")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBattleground([FromRoute] Guid id, [FromBody] UpdateBattleGroundRequest request)
        {
            return Response(await _battleGroundService.UpdateBattleGround(id, request));
        }
    }
}
