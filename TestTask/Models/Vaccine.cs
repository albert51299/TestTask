using System.ComponentModel.DataAnnotations;

namespace TestTask.Models {
    public class Vaccine {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
