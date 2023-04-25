using AirFinder.Application.Common;
using AirFinder.Domain.Users;
using JetBrains.Annotations;

namespace AirFinder.Application.Users.Models.Response
{
    public class CreateUserResponse : BaseResponse
    {
        public string? Email { get; set; }

        public static implicit operator CreateUserResponse(User user) 
        {
            if(user == null) return new CreateUserResponse();
            return new CreateUserResponse
            {
                Email = user.Person.Email
            };
        }
    }
}
