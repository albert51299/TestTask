using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            IEnumerable<Patient> patients = repository.GetAll();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            Patient patient = repository.Get(id);
            if (patient == null) {
                return NotFound();
            }

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

            if (repository.Get(patient.Id) == null) {
                return NotFound();
            }

            repository.Update(patient);
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
