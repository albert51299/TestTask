using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;

namespace TestTask.Controllers {
    /// <summary>
    /// Контроллер для действий с сущностью "Пациент".
    /// </summary>
    /// <remarks>
    /// Предоставляет методы чтения, создания, изменения, удаления пациентов.
    /// </remarks>
    [Route("api/patient-management/patients")]
    [ApiController]
    public class PatientController : ControllerBase {
        private readonly IDataRepository<Patient> repository;

        public PatientController(IDataRepository<Patient> dataRepository) {
            repository = dataRepository;
        }

        /// <summary>
        /// Чтение всех пациентов.
        /// </summary>
        /// <returns>HTTP ответ содержащий статус код и пациентов.</returns>
        /// <response code="200">Возвращает всех пациентов</response>
        [HttpGet]
        public ActionResult<IEnumerable<Patient>> Get() {
            IQueryable<Patient> patients = repository.GetAll();
            Log.Information($"{CurrentMethod.GetName()}: получены все пациенты");
            return Ok(patients);
        }

        /// <summary>
        /// Чтение пациента по его Id.
        /// </summary>
        /// <param name="id">Id пациента.</param>
        /// <returns>HTTP ответ содержащий статус код и пациента, или только статус код.</returns>
        /// <response code="200">Возвращает пациента</response>
        /// <response code="404">Ничего не возвращает</response>
        [HttpGet("{id}")]
        public ActionResult<Patient> Get(int id) {
            Patient patient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {id} отсутствует в базе данных");
                return NotFound();
            }

            Log.Information($"{CurrentMethod.GetName()}: получен пациент Id = {id}");
            return Ok(patient);
        }

        /// <summary>
        /// Добавление пациента.
        /// </summary>
        /// <param name="patient">Пациент.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
        /// <response code="200">Ничего не возвращает</response>
        /// <response code="400">Ничего не возвращает</response>
        /// <response code="409">Ничего не возвращает</response>
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

        /// <summary>
        /// Изменение данных пациента.
        /// </summary>
        /// <remarks>
        /// Перед изменением проверяется уникальность полученного номера СНИЛС.
        /// </remarks>
        /// <param name="patient">Новые данные пациента.</param>
        /// <param name="id">Id пациента.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
        /// <response code="200">Ничего не возвращает</response>
        /// <response code="400">Ничего не возвращает</response>
        /// <response code="404">Ничего не возвращает</response>
        /// <response code="409">Ничего не возвращает</response>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]Patient patient) {
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                return BadRequest();
            }

            Patient sameIdPatient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            if (sameIdPatient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {id} отсутствует в базе данных");
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
            Log.Information($"{CurrentMethod.GetName()}: изменен пациент Id = {id}");
            return Ok();
        }

        /// <summary>
        /// Удаление пациента.
        /// </summary>
        /// <param name="id">Id удаляемого пациента.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
        /// <response code="200">Ничего не возвращает</response>
        /// <response code="404">Ничего не возвращает</response>
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
