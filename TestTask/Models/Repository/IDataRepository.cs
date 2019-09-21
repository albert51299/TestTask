using System.Collections.Generic;

namespace TestTask.Models.Repository {
    public interface IDataRepository<TEntity> {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
