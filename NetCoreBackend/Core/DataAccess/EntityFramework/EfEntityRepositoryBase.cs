using Core.Entities;
using EFCore.BulkExtensions;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    // This class is implemented as partial classes:
    // - EfEntityRepositoryBase.Read.cs (Read operations)
    // - EfEntityRepositoryBase.AsyncRead.cs (Async Read operations)
    // - EfEntityRepositoryBase.Write.cs (Write operations)
    public partial class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        // Main class with implementation split across partial classes
        // This file contains no method implementations        // All implementations are in their respective partial class files:
        // - EfEntityRepositoryBase.Read.cs
        // - EfEntityRepositoryBase.AsyncRead.cs
        // - EfEntityRepositoryBase.Write.cs


    }
}
