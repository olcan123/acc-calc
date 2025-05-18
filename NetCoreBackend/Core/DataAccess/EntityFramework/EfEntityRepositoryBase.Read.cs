using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public partial class EfEntityRepositoryBase<TEntity, TContext> : IReadRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().FirstOrDefault(filter);
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(filter);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).AsNoTracking().ToList();
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            using TContext context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
                query = query.Include(include);

            return filter == null
                ? query.ToList()
                : query.Where(filter).ToList();
        }

        public List<TEntity> GetAllWithIncludeChain(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain, Expression<Func<TEntity, bool>> filter = null)
        {
            using TContext context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            query = includeChain(query);

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        public TEntity GetWithIncludeChain(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain, Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            var query = includeChain(context.Set<TEntity>());
            return query.FirstOrDefault(filter);
        }
    }
}
