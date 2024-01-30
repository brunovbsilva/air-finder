using AirFinder.Domain.Common;

namespace AirFinder.Domain.People.Models.Responses
{
    public class UpdateProfileResponse : BaseResponse
    {
        public string Token { get; set; } = String.Empty;
    }
}
