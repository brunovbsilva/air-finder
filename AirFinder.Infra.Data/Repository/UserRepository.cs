using AirFinder.Domain.People;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Infra.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<User> GetAll()
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();

            var query = (from u in tbUser
                         join p in tbPerson on u.IdPerson equals p.Id into people
                         from pp in people.DefaultIfEmpty()

                         select new User()
                         {
                             Id = u.Id,
                             Login = u.Login,
                             Password = u.Password,
                             Roll = u.Roll,
                             IdPerson = pp.Id,
                             Person = new Person()
                             {
                                 Id = pp.Id,
                                 Name = pp.Name,
                                 Email = pp.Email,
                                 Birthday = pp.Birthday,
                                 Gender = pp.Gender,
                                 CPF = pp.CPF,
                             }
                         });

            return query.AsQueryable();
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();

            var query = (from u in tbUser
                         join p in tbPerson on u.IdPerson equals p.Id into people
                         from pp in people.DefaultIfEmpty()

                         where (u.Login == login && !string.IsNullOrEmpty(login))
                         select new User()
                         {
                             Id = u.Id,
                             Login = u.Login,
                             Password = u.Password,
                             Roll = u.Roll,
                             IdPerson = pp.Id,
                             Person = new Person()
                             {
                                 Id = pp.Id,
                                 Name = pp.Name,
                                 Email = pp.Email,
                                 Birthday = pp.Birthday,
                                 Gender = pp.Gender,
                                 CPF = pp.CPF,
                             }
                         });
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();

            var query = (from u in tbUser
                         join p in tbPerson on u.IdPerson equals p.Id into people
                         from pp in people.DefaultIfEmpty()

                         where (pp.Email == email && !string.IsNullOrEmpty(email))
                         select new User()
                         {
                             Id = u.Id,
                             Login = u.Login,
                             Password = u.Password,
                             Roll = u.Roll,
                             IdPerson = pp.Id,
                             Person = new Person()
                             {
                                 Id = pp.Id,
                                 Name = pp.Name,
                                 Email = pp.Email,
                                 Birthday = pp.Birthday,
                                 Gender = pp.Gender,
                                 CPF = pp.CPF,
                             }
                         });
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User?> GetByCPFAsync(string cpf)
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();

            var query = (from u in tbUser
                         join p in tbPerson on u.IdPerson equals p.Id into people
                         from pp in people.DefaultIfEmpty()

                         where (pp.CPF == cpf && !string.IsNullOrEmpty(cpf))
                         select new User()
                         {
                             Id = u.Id,
                             Login = u.Login,
                             Password = u.Password,
                             Roll = u.Roll,
                             IdPerson = pp.Id,
                             Person = new Person()
                             {
                                 Id = pp.Id,
                                 Name = pp.Name,
                                 Email = pp.Email,
                                 Birthday = pp.Birthday,
                                 Gender = pp.Gender,
                                 CPF = pp.CPF,
                             }
                         });
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var tbPerson = _unitOfWork.Context.Set<Person>().AsNoTracking();
            var tbUser = _unitOfWork.Context.Set<User>().AsNoTracking();

            var query = (from u in tbUser
                         join p in tbPerson on u.IdPerson equals p.Id into people
                         from pp in people.DefaultIfEmpty()

                         where (u.Id == id)
                         select new User()
                         {
                             Id = u.Id,
                             Login = u.Login,
                             Password = u.Password,
                             Roll = u.Roll,
                             IdPerson = pp.Id,
                             Person = new Person()
                             {
                                 Id = pp.Id,
                                 Name = pp.Name,
                                 Email = pp.Email,
                                 Birthday = pp.Birthday,
                                 Gender = pp.Gender,
                                 CPF = pp.CPF,
                             }
                         });
            return await query.FirstOrDefaultAsync();
        } 
    }
}
