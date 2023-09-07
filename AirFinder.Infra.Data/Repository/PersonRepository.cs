using AirFinder.Domain.People;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
