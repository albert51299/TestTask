using System;

namespace TestTask.Models {
    /// <summary>
    /// Модель представления прививки.
    /// </summary>
    public class VaccinationVM {
        /// <summary>
        /// Id прививки в таблице "Прививки".
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название препарата используемого для прививки.
        /// </summary>
        public string VaccineName { get; set; }
        /// <summary>
        /// Наличие согласия пациента на прививку.
        /// </summary>
        public bool Consent { get; set; }
        /// <summary>
        /// Дата проведения прививки.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Id пациента в таблице "Пациенты".
        /// </summary>
        public int? PatientId { get; set; }
        /// <summary>
        /// Имя пациента.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество пациента.
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// Фамилия пациента.
        /// </summary>
        public string LastName { get; set; }
    }
}
