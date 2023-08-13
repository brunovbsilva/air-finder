using AirFinder.Domain.Common;

namespace AirFinder.Domain.Users.Models.Responses
{
    public class CreateUserResponse : BaseResponse
    {
        public string? Email { get; set; }

        public static implicit operator CreateUserResponse(User user) 
        {
            if(user == null) return new CreateUserResponse();
            return new CreateUserResponse
            {
                Email = user.Person?.Email
            };
        }
    }
}
