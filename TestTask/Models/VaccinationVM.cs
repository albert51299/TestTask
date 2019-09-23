using System;

namespace TestTask.Models {
    public class VaccinationVM {
        public int Id { get; set; }
        public string VaccineName { get; set; }
        public bool Consent { get; set; }
        public DateTime Date { get; set; }
        public int? PatientId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
    }
}
