using AirFinder.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AirFinder.Infra.Data
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IQueryable<TEntity> GetAll() => _unitOfWork.Context.Set<TEntity>().AsQueryable();

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression) => await GetAll().Where(expression).ToListAsync();
        public async Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> expression) => await GetAll().AsNoTracking().Where(expression).ToListAsync();

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression) => GetAll().Where(expression);

        public virtual async Task<TEntity?> GetByIDAsync(object id)
        {
            return await _unitOfWork.Context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task InsertWithSaveChangesAsync(TEntity entity)
        {
            await _unitOfWork.Context.Set<TEntity>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _unitOfWork.Context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity? entityToDelete = await _unitOfWork.Context.Set<TEntity>().FindAsync(id);
            await DeleteAsync(entityToDelete!);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (_unitOfWork.Context.Entry(entity).State == EntityState.Detached)
            {
                _unitOfWork.Context.Set<TEntity>().Attach(entity);
            }
            _unitOfWork.Context.Set<TEntity>().Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task UpdateWithSaveChangesAsync(TEntity entity)
        {
            _unitOfWork.Context.Set<TEntity>().Attach(entity);
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            _unitOfWork.Context.Set<TEntity>().Attach(entity);
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
