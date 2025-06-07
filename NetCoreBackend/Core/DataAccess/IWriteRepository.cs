using System.Collections.Generic;
using Core.Entities;
using EFCore.BulkExtensions;
using System;
using System.Linq.Expressions;
using LinqToDB;

namespace Core.DataAccess
{
    public interface IWriteRepository<T> where T : class, IEntity, new()
    {
        void Add(T entity);
        void AddRange(List<T> entityList);
        void Update(T entity);
        void Delete(T entity);
        void BulkAdd(List<T> entityList, BulkConfig bulkConfig = null);
        void BulkDelete(List<T> entityList, BulkConfig bulkConfig = null);
        void BulkUpdate(List<T> entityList, BulkConfig bulkConfig = null);
        void BulkAddOrUpdate(List<T> entityList, BulkConfig bulkConfig = null);
        void MergeLinq(List<T> entities, Expression<Func<T, T, bool>> matchExpression);
        void MergeLinqWithDelete(List<T> entities, Expression<Func<T, T, bool>> matchExpression, Expression<Func<T, T, bool>> deleteMatchExpression = null);
        void MergeSync(List<T> entities, Expression<Func<T, T, bool>> match, Expression<Func<T, bool>> deleteFilter = null);
        void MergeSync(List<T> entities, Expression<Func<T, T, bool>> match, Expression<Func<T, T, T>> updateAction = null, Expression<Func<T, T>> insertAction = null, Expression<Func<T, bool>> deleteFilter = null);
    }
}
