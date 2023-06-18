using AirFinder.Domain.Common;

namespace AirFinder.Domain.Users.Models.Responses
{
    public class LoginResponse : BaseResponse
    {
        public string? Token { get; set; }
    }
}
