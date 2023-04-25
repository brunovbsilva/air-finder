using Abp.Domain.Entities;
using AirFinder.Domain.Users.Enums;
using AirFinder.Domain.People;

namespace AirFinder.Domain.Users
{
    public class User : Entity
    {
        public User(string login, string password, int idPerson, UserRoll roll) 
        {
            Login = login;
            Password = password;
            IdPerson = idPerson;
            Roll = roll;
        }
        public User() {}
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public int IdPerson { get; set; } = 0;
        public UserRoll Roll { get; set; }
        public virtual Person Person { get; set; } = new Person();

    }
}
