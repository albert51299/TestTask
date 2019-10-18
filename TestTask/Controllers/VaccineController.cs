using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;

namespace TestTask.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase {
        private readonly IDataRepository<Vaccine> repository;

        public VaccineController(IDataRepository<Vaccine> dataRepository) {
            repository = dataRepository;
        }

        // получить препараты
        [HttpGet]
        public IActionResult Get() {
            IQueryable<Vaccine> vaccines = repository.GetAll();
            Log.Information($"{CurrentMethod.GetName()}: получены все препараты");
            return Ok(vaccines);
        }
    }
}
