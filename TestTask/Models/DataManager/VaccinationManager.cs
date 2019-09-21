using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestTask.Models.Repository;

namespace TestTask.Models.DataManager {
    public class VaccinationManager : IDataRepository<Vaccination> {
        readonly VaccinationsContext db;

        public VaccinationManager(VaccinationsContext context) {
            db = context;
        }

        public IEnumerable<Vaccination> GetAll() {
            return db.Vaccinations;
        }

        public Vaccination Get(int id) {
            return db.Vaccinations.FirstOrDefault(v => v.Id == id);
        }

        public void Add(Vaccination vaccination) {
            db.Add(vaccination);
            db.SaveChanges();
        }

        public void Update(Vaccination vaccination) {
            db.Entry(vaccination).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id) {
            Vaccination vaccination = db.Vaccinations.FirstOrDefault(v => v.Id == id);
            db.Vaccinations.Remove(vaccination);
            db.SaveChanges();
        }
    }
}
