using System.ComponentModel.DataAnnotations;

namespace TestTask.Models {
    /// <summary>
    /// Сущность "Препарат".
    /// </summary>
    public class Vaccine {
        /// <summary>
        /// Id препарата для хранения в базе данных.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название препарата.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
