using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;

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
            Log.Information($"{CurrentMethod.GetName()}: получены все пациенты");
            return Ok(patients);
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            Patient patient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {id} отсутствует в базе данных");
                return NotFound();
            }

            Log.Information($"{CurrentMethod.GetName()}: получен пациент Id = {id}");
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Patient patient) {
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                return BadRequest();
            }

            Patient sameSNILSPatient = repository.GetByCondition(p => p.SNILS == patient.SNILS).FirstOrDefault();
            if (sameSNILSPatient != null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент с таким СНИЛС уже есть");
                return Conflict();
            }
            
            repository.Add(patient);
            Log.Information($"{CurrentMethod.GetName()}: добавлен пациент Id = {patient.Id}");
            return Ok();
        }
        
        [HttpPut]
        public IActionResult Put([FromBody]Patient patient) {
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                return BadRequest();
            }

            Patient sameIdPatient = repository.GetByCondition(p => p.Id == patient.Id).FirstOrDefault();
            if (sameIdPatient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {patient.Id} отсутствует в базе данных");
                return NotFound();
            }

            // если был получен новый СНИЛС, проверить используется ли новый СНИЛС другим пациентом
            if (sameIdPatient.SNILS != patient.SNILS) {
                Patient sameSNILSPatient = repository.GetByCondition(p => p.SNILS == patient.SNILS).FirstOrDefault();
                if (sameSNILSPatient != null) {
                    Log.Information($"{CurrentMethod.GetName()}: пациент с таким СНИЛС уже есть");
                    return Conflict();
                }
            }

            repository.Update(patient);
            Log.Information($"{CurrentMethod.GetName()}: изменен пациент Id = {patient.Id}");
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Put(int id) {
            Patient patient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {id} отсутствует в базе данных");
                return NotFound();
            }

            repository.Delete(id);
            Log.Information($"{CurrentMethod.GetName()}: удален пациент Id = {id}");
            return Ok();
        }
    }
}
