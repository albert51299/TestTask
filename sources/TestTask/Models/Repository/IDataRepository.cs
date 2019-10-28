using System;
using System.Linq;
using System.Linq.Expressions;

namespace TestTask.Models.Repository {
    public interface IDataRepository<TEntity> {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetRangeByCondition(int skipCount, int takeCount, Expression<Func<TEntity, bool>> expression = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
