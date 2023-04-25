namespace AirFinder.Domain.People
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<Person> GetByEmailAsync(string email);
        Task<Person> GetByCPFAsync(string cpf);
    }
}
