using System;
using System.Linq;
using System.Linq.Expressions;
using ProjectManager.DataAccessLayer.Entity.Abtract;
using ProjectManager.DataAccessLayer.Repository.Helper;

namespace ProjectManager.DataAccessLayer.Repository.Abtract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> All();

        IQueryable<T> GetAll();

        T GetSingle(Guid key);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector);

        PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector,
            Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Edit(T entity);

        void Delete(T entity);

        void Save();
    }
}