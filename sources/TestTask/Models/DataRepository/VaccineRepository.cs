using System;
using System.Linq;
using System.Linq.Expressions;
using TestTask.Models.Repository;

namespace TestTask.Models.DataManager {
    public class VaccineRepository : IDataRepository<Vaccine> {
        readonly VaccinationsContext db;

        public VaccineRepository(VaccinationsContext context) {
            db = context;
        }

        public IQueryable<Vaccine> GetAll() {
            return db.Vaccines;
        }

        public void Add(Vaccine entity) {
            throw new NotImplementedException();
        }

        public void Delete(int id) {
            throw new NotImplementedException();
        }

        public IQueryable<Vaccine> GetByCondition(Expression<Func<Vaccine, bool>> expression) {
            throw new NotImplementedException();
        }

        public void Update(Vaccine entity) {
            throw new NotImplementedException();
        }

        public IQueryable<Vaccine> GetRangeByCondition(int skipCount, int takeCount, Expression<Func<Vaccine, bool>> expression = null) {
            throw new NotImplementedException();
        }
    }
}
