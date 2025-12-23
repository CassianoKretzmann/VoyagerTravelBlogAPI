using System.Linq.Expressions;

namespace VoyagerTravelBlog.Application.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "");
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> CreateListAsync(IEnumerable<TEntity> entities);
        Task RemoveAsync(TEntity entity);
        Task RemoveListAsync(IEnumerable<TEntity> entity);
        Task SaveAsync();
        Task UpdateAsync(TEntity entity);
    }
}
