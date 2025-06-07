using Core.Entities;
using EFCore.BulkExtensions;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public partial class EfEntityRepositoryBase<TEntity, TContext> : IWriteRepository<TEntity>
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
            context.Set<TEntity>().AddRange(entityList);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            using TContext context = new TContext();
            var deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            context.SaveChanges();
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
                .DeleteWhenMatchedAnd(deleteMatchExpression)
                .InsertWhenNotMatched()
                .UpdateWhenMatched()
                .Merge();

        }

        public void MergeSync(List<TEntity> entities, Expression<Func<TEntity, TEntity, bool>> match, Expression<Func<TEntity, bool>> deleteFilter = null)
        {
            using var context = new TContext();
            var table = context.Set<TEntity>().ToLinqToDBTable();

            var merge = table.Merge()
                .Using(entities)
                .On(match)
                .UpdateWhenMatchedAnd(match)
                .InsertWhenNotMatched();

            if (deleteFilter != null)
            {
                merge = merge.DeleteWhenNotMatchedBySourceAnd(deleteFilter);
            }

            merge.Merge();
        }

        public void MergeSync(List<TEntity> entities, Expression<Func<TEntity, TEntity, bool>> match, Expression<Func<TEntity, TEntity, TEntity>> updateAction = null, Expression<Func<TEntity, TEntity>> insertAction = null, Expression<Func<TEntity, bool>> deleteFilter = null)
        {
            using var context = new TContext();
            var table = context.Set<TEntity>().ToLinqToDBTable();

            var merge = table.Merge()
            .Using(entities)
            .On(match)
            .UpdateWhenMatched(updateAction)
            .InsertWhenNotMatched(insertAction);

            if (deleteFilter != null)
            {
                merge = merge.DeleteWhenNotMatchedBySourceAnd(deleteFilter);
            }

            merge.Merge();
        }
    }
}
