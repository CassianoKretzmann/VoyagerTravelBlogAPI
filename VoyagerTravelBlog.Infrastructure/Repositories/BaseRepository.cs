using VoyagerTravelBlog.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace VoyagerTravelBlog.Infrastructure.Repositories
{
    public class BaseRepository<TEntity>(BlogDbContext context) : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly BlogDbContext context = context;
        private readonly DbSet<TEntity> dbSet = context.Set<TEntity>();

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);

            await SaveAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> CreateListAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);

            await SaveAsync();

            return entities;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            var query = dbSet.Where(filter);

            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries)) 
            {
                query = query.Include(includeProperty.Trim());
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            var query = dbSet.Where(filter);

            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if(orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task RemoveAsync(TEntity entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task RemoveListAsync(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            await SaveAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
