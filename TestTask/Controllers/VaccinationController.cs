using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;

namespace TestTask.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController : ControllerBase {
        private readonly IDataRepository<VaccinationVM> repository;

        public VaccinationController(IDataRepository<VaccinationVM> dataRepository) {
            repository = dataRepository;
        }

        [HttpGet]
        public IActionResult Get() {
            IQueryable<VaccinationVM> vaccinations = repository.GetAll();
            return Ok(vaccinations);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetVaccinations")]
        public IActionResult GetVaccinations(int id) {
            IQueryable<VaccinationVM> vaccinations = repository.GetByCondition(v => v.PatientId == id);
            return Ok(vaccinations);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetVaccination")]
        public IActionResult GetVaccination(int id) {
            IQueryable<VaccinationVM> vaccinations = repository.GetByCondition(v => v.Id == id);
            if (vaccinations.Count() == 0) {
                return NotFound();
            }

            VaccinationVM vaccination = vaccinations.First();
            return Ok(vaccination);
        }

        [HttpPost]
        public IActionResult Post([FromBody]VaccinationVM vaccination) {
            if (vaccination == null) {
                return BadRequest();
            }

            repository.Add(vaccination);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]VaccinationVM vaccination) {
            if (vaccination == null) {
                return BadRequest();
            }

            IQueryable<VaccinationVM> vaccinations = repository.GetByCondition(v => v.Id == vaccination.Id);
            if (vaccinations.Count() == 0) {
                return NotFound();
            }

            repository.Update(vaccination);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            IQueryable<VaccinationVM> vaccinations = repository.GetByCondition(v => v.Id == id);
            if (vaccinations.Count() == 0) {
                return NotFound();
            }

            repository.Delete(id);
            return Ok();
        }
    }
}
