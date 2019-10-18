using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models {
    /// <summary>
    /// Сущность "Пациент".
    /// </summary>
    /// <remarks>
    /// Может иметь множество прививок.
    /// </remarks>
    public class Patient {
        /// <summary>
        /// Id пациента для хранения в базе данных.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя пациента.
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество пациента.
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// Фамилия пациента.
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Дата рождения пациента.
        /// </summary>
        [Column(TypeName = "DATE")]
        [Required]
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Пол пациента.
        /// </summary>
        [Required]
        public string Gender { get; set; }
        /// <summary>
        /// Страховой номер пациента.
        /// </summary>
        [Required]
        public string SNILS { get; set; } 
        /// <summary>
        /// Все прививки пациента.
        /// </summary>
        public ICollection<Vaccination> Vaccinations { get; set; }
    }
}
