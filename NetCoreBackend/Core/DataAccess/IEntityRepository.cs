using Core.Entities;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<T> GetAll(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        List<T> GetAllWithIncludeChain(Func<IQueryable<T>, IQueryable<T>> includeChain, Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        T Get(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        T GetWithIncludeChain(Func<IQueryable<T>, IQueryable<T>> includeChain, Expression<Func<T, bool>> filter);


        void Add(T entity);
        void AddRange(List<T> entityList);

        void Update(T entity);
        void Delete(T entity);

        void BulkAdd(List<T> entityList, BulkConfig bulkConfig = null);
        void BulkDelete(List<T> entityList, BulkConfig bulkConfig = null);
        void BulkUpdate(List<T> entityList, BulkConfig bulkConfig = null);
        void BulkAddOrUpdate(List<T> entityList, BulkConfig bulkConfig = null);
    }
}
