using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController : ControllerBase
    {
        private readonly IDataRepository<VaccinationVM> repository;

        public VaccinationController(IDataRepository<VaccinationVM> dataRepository)
        {
            repository = dataRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<VaccinationVM> vaccinations = repository.GetAll();
            Log.Information($"{CurrentMethod.GetName()}: получены все прививки");
            return Ok(vaccinations);
        }

        // получить все прививки пациента
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetVaccinations")]
        public IActionResult GetVaccinations(int id)
        {
            // добавить проверку наличия пациента в бд
            // для этого в этом контроллере нужно получить PatientRepository

            IQueryable<VaccinationVM> vaccinations = repository.GetByCondition(v => v.PatientId == id);
            Log.Information($"{CurrentMethod.GetName()}: получены все прививки для пациента PatientId = {id}");
            return Ok(vaccinations);
        }

        // получить вакцину
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetVaccination")]
        public IActionResult GetVaccination(int id)
        {
            VaccinationVM vaccination = repository.GetByCondition(v => v.Id == id).FirstOrDefault();
            if (vaccination == null)
            {
                Log.Information($"{CurrentMethod.GetName()}: прививка Id = {id} отсутствует в базе данных");
                return NotFound();
            }

            Log.Information($"{CurrentMethod.GetName()}: получена прививка Id = {id}");
            return Ok(vaccination);
        }

        [HttpPost]
        public IActionResult Post([FromBody]VaccinationVM vaccination)
        {
            if (vaccination == null)
            {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                return BadRequest();
            }

            repository.Add(vaccination);
            Log.Information($"{CurrentMethod.GetName()}: добавлена прививка Id = {vaccination.Id}");
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]VaccinationVM vaccination)
        {
            if (vaccination == null)
            {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                return BadRequest();
            }

            VaccinationVM vaccinationFromDB = repository.GetByCondition(v => v.Id == vaccination.Id).FirstOrDefault();
            if (vaccinationFromDB == null)
            {
                Log.Information($"{CurrentMethod.GetName()}: прививка Id = {vaccination.Id} отсутствует в базе данных");
                return NotFound();
            }

            repository.Update(vaccination);
            Log.Information($"{CurrentMethod.GetName()}: изменена прививка Id = {vaccination.Id}");
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            VaccinationVM vaccination = repository.GetByCondition(v => v.Id == id).FirstOrDefault();
            if (vaccination == null)
            {
                Log.Information($"{CurrentMethod.GetName()}: прививка Id = {id} отсутствует в базе данных");
                return NotFound();
            }

            repository.Delete(id);
            Log.Information($"{CurrentMethod.GetName()}: удалена прививка Id = {id}");
            return Ok();
        }
    }
}
