using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;
using TestTask.ViewModels;

namespace TestTask.Controllers {
    /// <summary>
    /// Контроллер для действий с сущностью "Пациент".
    /// </summary>
    /// <remarks>
    /// Предоставляет методы чтения, создания, изменения, удаления пациентов.
    /// </remarks>
    [ApiExplorerSettings(GroupName = "patient-management")]
    [Route("api/patient-management/patients")]
    [ApiController]
    public class PatientController : ControllerBase {
        private readonly IDataRepository<Patient> repository;

        public PatientController(IDataRepository<Patient> dataRepository) {
            repository = dataRepository;
        }

        /// <summary>
        /// Чтение пациентов.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Количество записей на одной странице.</param>
        /// <returns>HTTP ответ содержащий статус код и пациентов.</returns>
        /// <response code="200">Возвращает всех пациентов</response>
        /// <response code="400">Ничего не возвращает</response>
        [HttpGet]
        [Route("")]
        [Route("{page}/{page-size}")]
        public ActionResult<IEnumerable<Patient>> Get([FromRoute(Name = "page")]int page = 0, [FromRoute(Name = "page-size")]int pageSize = 10) {
            int skipCount = page * pageSize;
            int takeCount = pageSize;

            int patientsCount = repository.GetAll().Count();
            if (skipCount >= patientsCount || skipCount < 0 || takeCount <= 0) {
                Log.Information($"{CurrentMethod.GetName()}: неверный диапазон (начиная с {skipCount} выбрать {takeCount})");
                return BadRequest();
            }

            IQueryable<Patient> patients = repository.GetRangeByCondition(skipCount, takeCount);
            Log.Information($"{CurrentMethod.GetName()}: получены пациенты (начиная с {skipCount} выбрать {takeCount})");
            return Ok(patients);
        }

        /// <summary>
        /// Чтение пациента по его Id.
        /// </summary>
        /// <param name="id">Id пациента.</param>
        /// <returns>HTTP ответ содержащий статус код и пациента, или только статус код.</returns>
        /// <response code="200">Возвращает пациента</response>
        /// <response code="404">Ничего не возвращает</response>
        /// <response code="500">Ничего не возвращает</response>
        /// <example>
        /// Пример кода, вызывающего исключение System.ArgumentNullException.
        /// <code>
        /// Patient patient = repository.GetByCondition(null).FirstOrDefault();
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        [HttpGet("{id}")]
        public ActionResult<Patient> Get(int id) {
            Patient patient = null;
            try {
                patient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                return StatusCode(500);
            }

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
        /// <returns>HTTP ответ содержащий ответ сервиса.</returns>
        /// <response code="200">Возвращает ответ сервиса</response>
        /// <response code="400">Возвращает ответ сервиса</response>
        /// <response code="409">Возвращает ответ сервиса</response>
        /// <response code="500">Возвращает ответ сервиса</response>
        /// <example>
        /// Пример кода, вызывающего исключение System.ArgumentNullException.
        /// <code>
        /// Patient sameSNILSPatient = repository.GetByCondition(null).FirstOrDefault();
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        [HttpPost]
        public ActionResult<ResponseVM> Post([FromBody]Patient patient) {
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                ResponseVM response400 = new ResponseVM { IsSuccess = false, ErrorMessage = "Bad request", StatusCode = 400, Result = "Не удалось связать модель, patient был равен null" };
                return BadRequest(response400);
            }

            Patient sameSNILSPatient = null;
            try {
                sameSNILSPatient = repository.GetByCondition(p => p.SNILS == patient.SNILS).FirstOrDefault();
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            
            if (sameSNILSPatient != null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент с таким СНИЛС уже есть");
                ResponseVM response409 = new ResponseVM { IsSuccess = false, ErrorMessage = "Conflict", StatusCode = 409, Result = "Пациент с таким СНИЛС уже есть" };
                return Conflict(response409);
            }

            try {
                repository.Add(patient);
            }
            catch (DbUpdateConcurrencyException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            catch (DbUpdateException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }

            Log.Information($"{CurrentMethod.GetName()}: добавлен пациент Id = {patient.Id}");
            ResponseVM response200 = new ResponseVM { IsSuccess = true, StatusCode = 200, Result = $"Добавлен пациент Id = {patient.Id}" };
            return Ok(response200);
        }

        /// <summary>
        /// Изменение данных пациента.
        /// </summary>
        /// <remarks>
        /// Перед изменением проверяется уникальность полученного номера СНИЛС.
        /// </remarks>
        /// <param name="patient">Новые данные пациента.</param>
        /// <param name="id">Id пациента.</param>
        /// <returns>HTTP ответ содержащий ответ сервиса.</returns>
        /// <response code="200">Возвращает ответ сервиса</response>
        /// <response code="400">Возвращает ответ сервиса</response>
        /// <response code="404">Возвращает ответ сервиса</response>
        /// <response code="409">Возвращает ответ сервиса</response>
        /// <response code="500">Возвращает ответ сервиса</response>
        /// <example>
        /// Пример кода, вызывающего исключение System.ArgumentNullException.
        /// <code>
        /// Patient sameSNILSPatient = repository.GetByCondition(null).FirstOrDefault();
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        [HttpPut("{id}")]
        public ActionResult<ResponseVM> Put([FromRoute]int id, [FromBody]Patient patient) {
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                ResponseVM response400 = new ResponseVM { IsSuccess = false, ErrorMessage = "Bad request", StatusCode = 400, Result = "Не удалось связать модель, patient был равен null" };
                return BadRequest(response400);
            }

            Patient sameIdPatient = null;
            try {
                sameIdPatient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }

            if (sameIdPatient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {id} отсутствует в базе данных");
                ResponseVM response404 = new ResponseVM { IsSuccess = false, ErrorMessage = "Not found", StatusCode = 404, Result = $"Пациент Id = {id} отсутствует в базе данных" };
                return NotFound(response404);
            }

            // если был получен новый СНИЛС, проверить используется ли новый СНИЛС другим пациентом
            if (sameIdPatient.SNILS != patient.SNILS) {
                Patient sameSNILSPatient = null;
                try {
                    sameSNILSPatient = repository.GetByCondition(p => p.SNILS == patient.SNILS).FirstOrDefault();
                }
                catch (ArgumentNullException e) {
                    Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                    ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                    return StatusCode(500, response500);
                }

                if (sameSNILSPatient != null) {
                    Log.Information($"{CurrentMethod.GetName()}: пациент с таким СНИЛС уже есть");
                    ResponseVM response409 = new ResponseVM { IsSuccess = false, ErrorMessage = "Conflict", StatusCode = 409, Result = "Пациент с таким СНИЛС уже есть" };
                    return Conflict(response409);
                }
            }

            try {
                repository.Update(patient);
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            catch (DbUpdateConcurrencyException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            catch (DbUpdateException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }

            Log.Information($"{CurrentMethod.GetName()}: изменен пациент Id = {id}");
            ResponseVM response200 = new ResponseVM { IsSuccess = true, StatusCode = 200, Result = $"Изменен пациент Id = {id}" };
            return Ok(response200);
        }

        /// <summary>
        /// Удаление пациента.
        /// </summary>
        /// <param name="id">Id удаляемого пациента.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
        /// <response code="200">Ничего не возвращает</response>
        /// <response code="404">Ничего не возвращает</response>
        /// <response code="500">Ничего не возвращает</response>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        [HttpDelete("{id}")]
        public IActionResult Put(int id) {
            Patient patient = repository.GetByCondition(p => p.Id == id).FirstOrDefault();
            if (patient == null) {
                Log.Information($"{CurrentMethod.GetName()}: пациент Id = {id} отсутствует в базе данных");
                return NotFound();
            }

            try {
                repository.Delete(id);
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                return StatusCode(500);
            }
            catch (DbUpdateConcurrencyException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                return StatusCode(500);
            }
            catch (DbUpdateException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                return StatusCode(500);
            }

            Log.Information($"{CurrentMethod.GetName()}: удален пациент Id = {id}");
            return Ok();
        }
    }
}
