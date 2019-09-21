using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestTask.Models;
using TestTask.Models.Repository;

namespace TestTask.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController : ControllerBase {
        private readonly IDataRepository<Vaccination> repository;

        public VaccinationController(IDataRepository<Vaccination> dataRepository) {
            repository = dataRepository;
        }

        [HttpGet]
        public IActionResult Get() {
            IEnumerable<Vaccination> vaccinations = repository.GetAll();
            return Ok(vaccinations);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            Vaccination vaccination = repository.Get(id);
            if (vaccination == null) {
                return NotFound();
            }

            return Ok(vaccination);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Vaccination vaccination) {
            if (vaccination == null) {
                return BadRequest();
            }

            repository.Add(vaccination);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]Vaccination vaccination) {
            if (vaccination == null) {
                return BadRequest();
            }

            if (repository.Get(vaccination.Id) == null) {
                return NotFound();
            }

            repository.Update(vaccination);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Put(int id) {
            if (repository.Get(id) == null) {
                return NotFound();
            }

            repository.Delete(id);
            return Ok();
        }
    }
}
