namespace AirFinder.Infra.Data
{
    public interface IUnitOfWork
    {
        Context Context { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
