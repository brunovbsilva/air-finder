namespace AirFinder.Domain.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByCPFAsync(string cpf);
        Task<User?> GetByIdAsync(int id);
    }
}
