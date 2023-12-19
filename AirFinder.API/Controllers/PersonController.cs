using AirFinder.Application.People.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.People.Models.Requests;
using AirFinder.Domain.People.Models.Responses;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AirFinder.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;
        public PersonController(
            INotification notification,
            IPersonService personService
        ) : base(notification)
        {
            _personService = personService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Search for people with any match for name, email or phone")]
        [ProducesResponseType(typeof(SearchPeopleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchPeople([FromQuery] SearchPeopleRequest request)
        {
            return Response(await _personService.Search(request));
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update Profile details")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            return Response(await _personService.Update(request, GetUserId(HttpContext)));
        }

        [HttpGet("details/{personId}")]
        [SwaggerOperation(Summary = "Get details from a person")]
        [ProducesResponseType(typeof(GetPersonDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Details(Guid personId)
        {
            return Response(await _personService.Details(personId));
        }
    }
}
