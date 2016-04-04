using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using ProjectManager.DataAccessLayer.Entity.Abtract;
using ProjectManager.DataAccessLayer.Extension;
using ProjectManager.DataAccessLayer.Repository.Abtract;
using ProjectManager.DataAccessLayer.Repository.Helper;

namespace ProjectManager.DataAccessLayer.Repository
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntity, new()
    {
        private readonly DbContext _entityContext;

        public EntityRepository(DbContext entityContext)
        {
            if (entityContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            _entityContext = entityContext;
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entityContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IQueryable<T> All()
        {
            return GetAll();
        }

        public T GetSingle(Guid key)
        {
            return GetAll().FirstOrDefault(x => x.Key == key);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _entityContext.Set<T>().Where(predicate);
        }

        public virtual PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector)
        {
            return Paginate(pageIndex, pageSize, keySelector, null, null);
        }

        public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query =
                AllIncluding(includeProperties).OrderBy(keySelector);
            query = (predicate == null)
                ? query
                : query.Where(predicate);
            return query.ToPaginatedList(pageIndex, pageSize);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _entityContext.Entry(entity);
            _entityContext.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = _entityContext.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _entityContext.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void Save()
        {
            _entityContext.SaveChanges();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entityContext.Set<T>();
        }
    }
}