using AirFinder.Domain.Users.Enums;
using AirFinder.Domain.People;
using AirFinder.Domain.Common;
using AirFinder.Domain.Users.Models.Requests;

namespace AirFinder.Domain.Users
{
    public class User : BaseModel
    {
        public User(string login, string password, Guid idPerson, UserRoll roll) 
        {
            Login = login;
            Password = password;
            IdPerson = idPerson;
            Roll = roll;
            Person = null;
        }

        public User(UserRequest request)
        {
            Login = request.Login.ToLower();
            Password = request.Password;
            Roll = UserRoll.Default;
            Person = new Person(
                request.Name,
                request.Email.ToLower(),
                request.Birthday,
                request.CPF,
                request.Gender,
                request.Phone
            );
        }

        public User(UserAdminRequest request)
        {
            Login = request.Login.ToLower();
            Password = request.Password;
            Roll = request.Role;
            Person = new Person(
                request.Name,
                request.Email.ToLower(),
                request.Birthday,
                request.CPF,
                request.Gender,
                request.Phone
            );
        }

        public User() {}
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public Guid IdPerson { get; set; } = new Guid();
        public UserRoll Roll { get; set; }
        public virtual Person? Person { get; set; } = null;

    }
}
