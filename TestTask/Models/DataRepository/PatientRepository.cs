using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using TestTask.Models.Repository;

namespace TestTask.Models.DataManager {
    public class PatientRepository : IDataRepository<Patient> {
        readonly VaccinationsContext db;

        public PatientRepository(VaccinationsContext context) {
            db = context;
        }

        public IQueryable<Patient> GetAll() {
            return db.Patients;
        }

        public IQueryable<Patient> GetByCondition(Expression<Func<Patient, bool>> expression) {
            return db.Patients.Where(expression);
        }

        public void Add(Patient patient) {
            db.Patients.Add(patient);
            db.SaveChanges();
        }

        public void Update(Patient patient) {
            Patient fromDB = db.Patients.FirstOrDefault(p => p.Id == patient.Id);
            fromDB.FirstName = patient.FirstName;
            fromDB.SecondName = patient.SecondName;
            fromDB.LastName = patient.LastName;
            fromDB.DateOfBirth = patient.DateOfBirth;
            fromDB.Gender = patient.Gender;
            fromDB.SNILS = patient.SNILS;
            db.Entry(fromDB).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id) {
            Patient patient = db.Patients.FirstOrDefault(p => p.Id == id);
            db.Patients.Remove(patient);
            db.SaveChanges();
        }
    }
}
