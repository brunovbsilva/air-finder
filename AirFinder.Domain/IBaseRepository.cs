using System.Linq.Expressions;

namespace AirFinder.Domain
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity?> GetByIDAsync(object id);

        Task InsertWithSaveChangesAsync(TEntity entity);

        Task InsertAsync(TEntity entity);

        Task DeleteAsync(object id);

        Task DeleteAsync(TEntity entity);

        Task UpdateWithSaveChangesAsync(TEntity entity);

        void Update(TEntity entity);

        Task SaveChangesAsync();
    }
}
