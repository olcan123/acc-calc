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


        void AddWithExistingRelation<TRelated>(T entityToAdd, object relatedEntityId, string navigationPropertyName) where TRelated : class, new();
        void Add(T entity);
        void AddWithChildren(T entity);

        void Update(T entity);
        void UpdateWithChildren(T entity);
        void InsertOrUpdateWithChildren(T entity);
        void Delete(T entity);

        void BulkAdd(List<T> entityList);
        void BulkDelete(List<T> entityList);
        void BulkUpdate(List<T> entityList);
    }
}
