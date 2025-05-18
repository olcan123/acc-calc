using Core.Entities;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> : IReadRepository<T>, IWriteRepository<T>, IAsyncReadRepository<T>
        where T : class, IEntity, new()
    {
        // This interface combines read, write, and async read operations
        // All methods are inherited from the base interfaces

    }
}
