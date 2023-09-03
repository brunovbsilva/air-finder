using AirFinder.Domain.People;
using AirFinder.Domain.People.Enums;

namespace AirFinder.Application.Tests.Mocks
{
    public class PersonMocks
    {
        public static Person PersonDefault()
        {
            return new Person()
            {
                Id = It.IsAny<Guid>(),
                Name = It.IsAny<string>(),
                Email = It.IsAny<string>(),
                Birthday = It.IsAny<DateTime>(),
                CPF = It.IsAny<string>(),
                Gender = It.IsAny<Gender>(),
                Phone = It.IsAny<string>()
            };
        }
        public static IEnumerable<Person> PersonDefaultEnumerable()
        {
            return new EnumerableQuery<Person>( new List<Person> { PersonDefault() } );
        }
        public static IEnumerable<Person> PersonDefaultEmptyEnumerable()
        {
            return new EnumerableQuery<Person>( new List<Person>() );
        }
    }
}
