using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;

namespace TestTask.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase {
        private readonly IDataRepository<Patient> repository;

        public PatientController(IDataRepository<Patient> dataRepository) {
            repository = dataRepository;
        }

        [HttpGet]
        public IActionResult Get() {
            IQueryable<Patient> patients = repository.GetAll();
            return Ok(patients);
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            IQueryable<Patient> patients = repository.GetByCondition(p => p.Id == id);
            if (patients.Count() == 0) {
                return NotFound();
            }

            Patient patient = patients.First();
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Patient patient) {
            if (patient == null) {
                return BadRequest();
            }

            repository.Add(patient);
            return Ok();
        }
        
        [HttpPut]
        public IActionResult Put([FromBody]Patient patient) {
            if (patient == null) {
                return BadRequest();
            }

            IQueryable<Patient> patients = repository.GetByCondition(p => p.Id == patient.Id);
            if (patients.Count() == 0) {
                return NotFound();
            }

            repository.Update(patient);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Put(int id) {
            IQueryable<Patient> patients = repository.GetByCondition(p => p.Id == id);
            if (patients.Count() == 0) {
                return NotFound();
            }

            repository.Delete(id);
            return Ok();
        }
    }
}
