using AirFinder.Domain.Users.Enums;
using AirFinder.Domain.People;

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
        }
        public User() {}
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public Guid IdPerson { get; set; } = new Guid();
        public UserRoll Roll { get; set; }
        public virtual Person Person { get; set; } = new Person();

    }
}
