using AirFinder.Application.BattleGrounds.Services;
using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetBattlegrounds()
        {
            return Response(await _battleGroundService.GetBattleGrounds(GetUserId(HttpContext)));
        }
        [HttpPost]
        public async Task<IActionResult> CreateBattleground([FromBody] CreateBattleGroundRequest request)
        {
            return Response(await _battleGroundService.CreateBattleGround(GetUserId(HttpContext), request));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBattleground(Guid id)
        {
            return Response(await _battleGroundService.DeleteBattleGround(id));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBattleground([FromRoute] Guid id, [FromBody] UpdateBattleGroundRequest request)
        {
            return Response(await _battleGroundService.UpdateBattleGround(id, request));
        }
    }
}
