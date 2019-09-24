using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;

namespace TestTask.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase {
        private readonly IDataRepository<Vaccine> repository;

        public VaccineController(IDataRepository<Vaccine> dataRepository) {
            repository = dataRepository;
        }

        [HttpGet]
        public IActionResult Get() {
            IQueryable<Vaccine> vaccines = repository.GetAll();
            return Ok(vaccines);
        }
    }
}
