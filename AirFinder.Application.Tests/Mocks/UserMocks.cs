using AirFinder.Domain.People;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;
using System.Data;

namespace AirFinder.Application.Tests.Mocks
{
    public class UserMocks
    {
        public static User Default()
        {
            var person = PersonMocks.Default();
            return new User(
                It.IsAny<string>(),
                It.IsAny<string>(),
                person.Id,
                It.IsAny<UserRole>()
            )
            {
                Person = person
            };
        }
        public static IEnumerable<User> DefaultEnumerable()
        {
            return new EnumerableQuery<User>( new List<User> { Default() } );
        }
        public static IEnumerable<User> DefaultEmptyEnumerable()
        {
            return new EnumerableQuery<User>(new List<User>());
        }
    }
}
