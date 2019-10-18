﻿using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;

namespace TestTask.Controllers
{
    /// <summary>
    /// Контроллер для действий с сущностью "Прививка".
    /// </summary>
    /// <remarks>
    /// Предоставляет методы чтения, создания, изменения, удаления прививок.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController : ControllerBase
    {
        private readonly IDataRepository<VaccinationVM> repository;

        public VaccinationController(IDataRepository<VaccinationVM> dataRepository)
        {
            repository = dataRepository;
        }

        /// <summary>
        /// Чтение всех прививок.
        /// </summary>
        /// <returns>HTTP ответ содержащий статус код и прививки.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<VaccinationVM> vaccinations = repository.GetAll();
            Log.Information($"{CurrentMethod.GetName()}: получены все прививки");
            return Ok(vaccinations);
        }

        /// <summary>
        /// Чтение всех прививок конкретного пациента.
        /// </summary>
        /// <param name="id">Id пациента.</param>
        /// <returns>HTTP ответ содержащий статус код и прививки.</returns>
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

        /// <summary>
        /// Чтение прививки по ее Id.
        /// </summary>
        /// <param name="id">Id прививки.</param>
        /// <returns>HTTP ответ содержащий статус код и прививку, или только статус код.</returns>
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

        /// <summary>
        /// Добавление прививки.
        /// </summary>
        /// <param name="vaccination">Прививка.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
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

        /// <summary>
        /// Изменение данных прививки.
        /// </summary>
        /// <param name="vaccination">Новые данные прививки.</param>
        /// <returns>HTTP ответ со статус-кодом.</returns>
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

        /// <summary>
        /// Удаление прививки.
        /// </summary>
        /// <param name="id">Id удаляемой прививки.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
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