﻿using AirFinder.Domain.People;
using AirFinder.Domain.People.Enums;

namespace AirFinder.Application.Tests.Mocks
{
    public class PersonMocks
    {
        public static Person Default()
        {
            return new Person(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<Gender>(),
                It.IsAny<string>()
            )
            {
                Id = It.IsAny<Guid>()
            };
        }
        public static IEnumerable<Person> DefaultEnumerable()
        {
            return new EnumerableQuery<Person>( new List<Person> { Default() } );
        }
        public static IEnumerable<Person> DefaultEmptyEnumerable()
        {
            return new EnumerableQuery<Person>( new List<Person>() );
        }
    }
}
