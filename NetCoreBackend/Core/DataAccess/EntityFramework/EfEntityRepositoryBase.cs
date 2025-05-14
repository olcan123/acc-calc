using Core.Entities;
using EFCore.BulkExtensions;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {

        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void AddRange(List<TEntity> entityList)
        {
            using TContext context = new TContext();
            entityList.AddRange(entityList);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            using TContext context = new TContext();
            var deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

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

        public TEntity GetWithIncludeChain(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain,
            Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            var query = includeChain(context.Set<TEntity>());
            return query.FirstOrDefault(filter);
        }




        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void BulkAdd(List<TEntity> entityList, BulkConfig bulkConfig = null)
        {
            using TContext context = new TContext();
            context.BulkInsert(entityList, bulkConfig);
        }

        public void BulkDelete(List<TEntity> entityList, BulkConfig bulkConfig = null)
        {
            using TContext context = new TContext();
            context.BulkDelete(entityList, bulkConfig);
        }


        public void BulkUpdate(List<TEntity> entityList, BulkConfig bulkConfig = null)
        {
            using TContext context = new TContext();
            context.BulkUpdate(entityList, bulkConfig);
        }

        public void BulkAddOrUpdate(List<TEntity> entityList, BulkConfig bulkConfig = null)
        {
            using TContext context = new TContext();
            context.BulkInsertOrUpdate(entityList, bulkConfig);
        }

        //SECTION - LINQ2DB

        public void MergeLinq(List<TEntity> entities, Expression<Func<TEntity, TEntity, bool>> matchExpression)
        {

            using var context = new TContext();
            context.Set<TEntity>().ToLinqToDBTable()
                .Merge()
                .Using(entities)
                .On(matchExpression)
                .InsertWhenNotMatched()
                .UpdateWhenMatched()
                .Merge();
        }

        public void MergeLinqWithDelete(
            List<TEntity> entities,
            Expression<Func<TEntity, TEntity, bool>> matchExpression,
            Expression<Func<TEntity, TEntity, bool>> deleteMatchExpression = null)
        {
            if (deleteMatchExpression == null)
                deleteMatchExpression = matchExpression;

            using var context = new TContext();
            context.Set<TEntity>().ToLinqToDBTable()
                .Merge()
                .Using(entities)
                .On(matchExpression)
                .InsertWhenNotMatched()
                .UpdateWhenMatched()
                .DeleteWhenMatchedAnd(deleteMatchExpression)
                .Merge();
        }


    }
}
