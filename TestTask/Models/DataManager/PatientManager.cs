using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestTask.Models.Repository;

namespace TestTask.Models.DataManager {
    public class PatientManager : IDataRepository<Patient> {
        readonly VaccinationsContext db;

        public PatientManager(VaccinationsContext context) {
            db = context;
        }

        public IEnumerable<Patient> GetAll() {
            return db.Patients;
        }

        public Patient Get(int id) {
            return db.Patients.FirstOrDefault(p => p.Id == id);
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
