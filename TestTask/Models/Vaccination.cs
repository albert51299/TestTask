using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models {
    public class Vaccination {
        public int Id { get; set; }
        [Required]
        public string VaccineName { get; set; }
        [Required]
        public bool Consent { get; set; }
        [Column(TypeName = "DATE")]
        [Required]
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
