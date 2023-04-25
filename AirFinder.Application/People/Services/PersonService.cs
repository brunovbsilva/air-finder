using AirFinder.Application.Common;
using AirFinder.Application.Email.Services;
using AirFinder.Domain.People;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Application.People.Services
{
    public class PersonService : BaseService, IPersonService
    {
        readonly IPersonRepository _personRepository;
        public PersonService(
            IMailService mailService,
            IPersonRepository personRepository) : base(mailService) 
        {
            _personRepository = personRepository;
        }
        public async Task<List<Person>> GetAll()
        {
            return await _personRepository.GetAll().ToListAsync();
        }
        
        public async Task<Person> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) 
                throw new ArgumentException(nameof(email));

            return await _personRepository.GetByEmailAsync(email);
        }

        public async Task<Person> GetByCPFAsync(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                throw new ArgumentException(nameof(cpf));

            return await _personRepository.GetByCPFAsync(cpf);
        }

        public async Task<Person> GetById(int id)
        {
            return await _personRepository.GetByIDAsync(id);
        }

        public async Task<BaseResponse> Delete(int id)
        {
            var person = await _personRepository.GetByIDAsync(id) ?? throw new ArgumentException("User not found.");
            await _personRepository.DeleteAsync(person);
            return new GenericResponse();
        }

        public Task<Person> Insert(Person item)
        {
            throw new NotImplementedException();
        }
    }
}
