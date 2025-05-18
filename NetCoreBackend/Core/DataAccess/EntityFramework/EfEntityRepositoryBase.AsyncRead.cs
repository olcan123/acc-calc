using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public partial class EfEntityRepositoryBase<TEntity, TContext> : IAsyncReadRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetWithIncludeChainAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain, Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            var query = includeChain(context.Set<TEntity>());
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            using var context = new TContext();
            return filter == null
                ? await context.Set<TEntity>().ToListAsync()
                : await context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
                query = query.Include(include);

            return filter == null
                ? await query.ToListAsync()
                : await query.Where(filter).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllWithIncludeChainAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain, Expression<Func<TEntity, bool>> filter = null)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            query = includeChain(query);

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }
    }
}
