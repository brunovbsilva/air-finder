using AirFinder.Domain.People;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
