using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.DataAccess
{
    public interface IReadRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);
        T Get(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        T GetWithIncludeChain(Func<IQueryable<T>, IQueryable<T>> includeChain, Expression<Func<T, bool>> filter);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<T> GetAll(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        List<T> GetAllWithIncludeChain(Func<IQueryable<T>, IQueryable<T>> includeChain, Expression<Func<T, bool>> filter = null);
    }
}
