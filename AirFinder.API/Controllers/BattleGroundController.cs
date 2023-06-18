using AirFinder.Application.BattleGrounds.Services;
using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirFinder.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BattleGroundController : BaseController
    {
        private readonly IBattleGroundService _battleGroundService;
        public BattleGroundController(INotification notification, IBattleGroundService battleGroundService) : base(notification)
        {
            _battleGroundService = battleGroundService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBattleGrounds()
        {
            return Response(await _battleGroundService.GetBattleGrounds(GetUserId(HttpContext)));
        }
        [HttpPost]
        public async Task<IActionResult> CreateBattleGround([FromBody] CreateBattleGroundRequest request)
        {
            return Response(await _battleGroundService.CreateBattleGround(GetUserId(HttpContext), request));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBattleGround(Guid id)
        {
            return Response(await _battleGroundService.DeleteBattleGround(id));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBattleGround([FromRoute] Guid id, [FromBody] UpdateBattleGroundRequest request)
        {
            return Response(await _battleGroundService.UpdateBattleGround(id, request));
        }
    }
}
