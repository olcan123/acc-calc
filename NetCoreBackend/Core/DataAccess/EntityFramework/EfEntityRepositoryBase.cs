using Core.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {

        public void AddWithExistingRelation<TRelated>(TEntity entityToAdd, object relatedEntityId, string navigationPropertyName) where TRelated : class, new()
        {
            using var context = new TContext();

            // Mevcut ilişkilendirilecek entity (örnek: Company)
            var relatedEntity = new TRelated();
            var keyName = context.Model.FindEntityType(typeof(TRelated))
                                .FindPrimaryKey()
                                .Properties
                                .First().Name;

            context.Entry(relatedEntity).Property(keyName).CurrentValue = relatedEntityId;
            context.Set<TRelated>().Attach(relatedEntity);

            // İlişkiyi kur
            var navCollection = context.Entry(entityToAdd).Collection(navigationPropertyName);
            navCollection.Load();

            if (navCollection.CurrentValue is ICollection<TRelated> collection)
            {
                collection.Add(relatedEntity);
            }
            else
            {
                throw new InvalidOperationException($"{navigationPropertyName} is not a collection.");
            }

            context.Set<TEntity>().Add(entityToAdd);
            context.SaveChanges();
        }



        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void AddWithChildren(TEntity entity)
        {
            using TContext context = new TContext();

            context.ChangeTracker.TrackGraph(entity, e =>
            {
                if (e.Entry.State == EntityState.Detached)
                {
                    e.Entry.State = EntityState.Added;
                }
            });

            context.SaveChanges();
        }

        public void UpdateWithChildren(TEntity entity)
        {
            using TContext context = new TContext();
            context.Attach(entity);

            context.Entry(entity).State = EntityState.Modified;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // Burada sadece değişen property'leri Modified yapabilirsin
                // Örnek: entry.Property("Name").IsModified = true;
                // veya tümünü işaretle:
                entry.State = EntityState.Modified;
            }

            context.SaveChanges();
        }



        public void InsertOrUpdateWithChildren(TEntity entity)
        {
            using var context = new TContext();

            // Entity ve tüm ilişkili nesneleri ChangeTracker'a dahil et
            context.Attach(entity);

            foreach (var entry in context.ChangeTracker.Entries())
            {
                var primaryKey = entry.Metadata.FindPrimaryKey();

                bool isNew = false;

                if (primaryKey != null)
                {
                    // Eğer PK değeri temporary (örneğin EF tarafından atanmış ama kalıcı değilse) veya default ise => Added
                    isNew = primaryKey.Properties.Any(p =>
                    {
                        var propEntry = entry.Property(p.Name);
                        return propEntry.IsTemporary || IsDefaultValue(propEntry.CurrentValue);
                    });
                }

                entry.State = isNew ? EntityState.Added : EntityState.Modified;
            }

            context.SaveChanges();
        }

        private static bool IsDefaultValue(object value)
        {
            if (value == null)
                return true;

            var type = value.GetType();
            return value.Equals(type.IsValueType ? Activator.CreateInstance(type) : null);
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

        public TEntity GetWithIncludeChain(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain, Expression<Func<TEntity, bool>> filter)
        {
            using TContext context = new TContext();

            IQueryable<TEntity> query = context.Set<TEntity>();
            query = includeChain(query);

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

        public void BulkAdd(List<TEntity> entityList)
        {
            using TContext context = new TContext();
            context.BulkInsert(entityList);

        }

        public void BulkDelete(List<TEntity> entityList)
        {
            using TContext context = new TContext();
            context.BulkDelete(entityList);
        }


        public void BulkUpdate(List<TEntity> entityList)
        {
            using TContext context = new TContext();
            context.BulkUpdate(entityList);
        }

    }
}
