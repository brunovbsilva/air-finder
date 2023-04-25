using AirFinder.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AirFinder.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected BaseController() { }

        private bool IsValidOperation(BaseResponse response) => response.Success == true;

        protected new IActionResult Response(BaseResponse response)
        {
            if (response == null) response = new GenericResponse { Success = false, Error = "Unknow Error" };
            if (IsValidOperation(response))
            {
                return Ok(response);
            }

            response = new GenericResponse { Success = response.Success, Error = response.Error };
            return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }
    }
}
