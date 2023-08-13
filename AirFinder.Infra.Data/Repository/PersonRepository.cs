using AirFinder.Domain.People;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Infra.Data.Repository
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public PersonRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            _unitOfWork = unitOfWork;        
        }

        public async Task<Person?> GetByCPFAsync(string cpf)
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();

            var query = (from p in tbPerson.DefaultIfEmpty()
                         where (p.CPF == cpf && !string.IsNullOrEmpty(cpf))
                         select p);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Person?> GetByEmailAsync(string email)
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();

            var query = (from p in tbPerson.DefaultIfEmpty()
                         where (p.Email == email && !string.IsNullOrEmpty(email))
                         select p);

            return await query.FirstOrDefaultAsync();
        }
    }
}
