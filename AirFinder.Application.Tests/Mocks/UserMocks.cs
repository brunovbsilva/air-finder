using AirFinder.Domain.People;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;

namespace AirFinder.Application.Tests.Mocks
{
    public class UserMocks
    {
        public static User UserDefault()
        {
            var person = PersonMocks.PersonDefault();
            return new User()
            {
                Login = It.IsAny<string>(),
                Password = It.IsAny<string>(),
                IdPerson = person.Id,
                Role = It.IsAny<UserRole>(),
                Person = person
            };
        }
        public static IEnumerable<User> UserDefaultEnumerable()
        {
            return new EnumerableQuery<User>( new List<User> { UserDefault() } );
        }
        public static IEnumerable<User> UserDefaultEmptyEnumerable()
        {
            return new EnumerableQuery<User>(new List<User>());
        }
    }
}
