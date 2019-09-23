using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using TestTask.Models.Repository;

namespace TestTask.Models.DataManager {
    public class VaccinationManager : IDataRepository<VaccinationVM> {
        readonly VaccinationsContext db;

        public VaccinationManager(VaccinationsContext context) {
            db = context;
        }

        public IQueryable<VaccinationVM> GetAll() {
            IQueryable<VaccinationVM> vaccinations = db.VaccinationVMs
                .FromSql("SELECT Vaccinations.Id, Vaccinations.VaccineName, Vaccinations.Consent, Vaccinations.Date, Vaccinations.PatientId, Patients.FirstName, Patients.SecondName, Patients.LastName FROM Vaccinations " +
                "LEFT JOIN Patients ON Patients.Id = Vaccinations.PatientId ORDER BY Vaccinations.Id");
            return vaccinations;
        }

        public IQueryable<VaccinationVM> GetByCondition(Expression<Func<VaccinationVM, bool>> expression) {
            IQueryable<VaccinationVM> allVaccinations = db.VaccinationVMs
                .FromSql("SELECT Vaccinations.Id, Vaccinations.VaccineName, Vaccinations.Consent, Vaccinations.Date, Vaccinations.PatientId, Patients.FirstName, Patients.SecondName, Patients.LastName FROM Vaccinations " +
                "LEFT JOIN Patients ON Patients.Id = Vaccinations.PatientId");
            IQueryable<VaccinationVM> vaccinations = allVaccinations.Where(expression);
            return vaccinations;
        }

        public void Add(VaccinationVM vaccination) {
            db.Vaccinations.Add(new Vaccination { VaccineName = vaccination.VaccineName, Consent = vaccination.Consent, Date = vaccination.Date, PatientId = vaccination.PatientId });
            db.SaveChanges();
        }

        public void Update(VaccinationVM vaccination) {
            Vaccination fromDB = db.Vaccinations.FirstOrDefault(v => v.Id == vaccination.Id);
            fromDB.VaccineName = vaccination.VaccineName;
            fromDB.Consent = vaccination.Consent;
            fromDB.Date = vaccination.Date;
            db.Entry(fromDB).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id) {
            Vaccination vaccination = db.Vaccinations.FirstOrDefault(v => v.Id == id);
            db.Vaccinations.Remove(vaccination);
            db.SaveChanges();
        }
    }
}
