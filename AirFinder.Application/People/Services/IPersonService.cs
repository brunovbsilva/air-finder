using AirFinder.Application.Common;
using AirFinder.Domain.People;

namespace AirFinder.Application.People.Services
{
    public interface IPersonService : IGenericService<Person>
    {
        Task<Person> GetByEmailAsync(string email);
        Task<Person> GetByCPFAsync(string cpf);
    }
}
