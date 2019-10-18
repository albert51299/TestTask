using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models {
    /// <summary>
    /// Сущность "Прививка".
    /// </summary>
    /// <remarks>
    /// Может быть связана с сущностью "Пациент".
    /// </remarks>
    public class Vaccination {
        /// <summary>
        /// Id прививки для хранения в базе данных.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название препарата используемого для прививки.
        /// </summary>
        [Required]
        public string VaccineName { get; set; }
        /// <summary>
        /// Наличие согласия пациента на прививку.
        /// </summary>
        [Required]
        public bool Consent { get; set; }
        /// <summary>
        /// Дата проведения прививки.
        /// </summary>
        [Column(TypeName = "DATE")]
        [Required]
        public DateTime Date { get; set; }
        /// <summary>
        /// Id пациента в таблице "Пациенты".
        /// </summary>
        public int? PatientId { get; set; }
        /// <summary>
        /// Сущность "Пациент".
        /// </summary>
        public Patient Patient { get; set; }
    }
}
