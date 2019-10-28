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
    /// Контроллер для действий с сущностью "Прививка".
    /// </summary>
    /// <remarks>
    /// Предоставляет методы чтения, создания, изменения, удаления прививок.
    /// </remarks>
    [ApiExplorerSettings(GroupName = "vaccination-management")]
    [Route("api/vaccination-management")]
    [ApiController]
    public class VaccinationController : ControllerBase {
        private readonly IDataRepository<VaccinationVM> repository;

        public VaccinationController(IDataRepository<VaccinationVM> dataRepository) {
            repository = dataRepository;
        }

        /// <summary>
        /// Чтение всех прививок.
        /// </summary>
        /// <returns>HTTP ответ содержащий статус код и прививки.</returns>
        /// <response code="200">Возвращает все прививки</response>
        [HttpGet]
        [Route("vaccinations")]
        public ActionResult<IEnumerable<Vaccination>> Get() {
            IQueryable<VaccinationVM> vaccinations = repository.GetAll();
            Log.Information($"{CurrentMethod.GetName()}: получены все прививки");
            return Ok(vaccinations);
        }

        /// <summary>
        /// Чтение всех прививок конкретного пациента.
        /// </summary>
        /// <param name="patientId">Id пациента.</param>
        /// <returns>HTTP ответ содержащий статус код и прививки.</returns>
        /// <response code="200">Возвращает все прививки пациента</response>
        /// <response code="500">Ничего не возвращает</response>
        /// <example>
        /// Пример кода, вызывающего исключение System.ArgumentNullException.
        /// <code>
        /// vaccinations = repository.GetByCondition(null);
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        [HttpGet]
        [Route("patients/{patient-id}/vaccinations")]
        public ActionResult<IEnumerable<Vaccination>> GetVaccinations([FromRoute(Name = "patient-id")]int patientId) {
            // добавить проверку наличия пациента в бд
            // для этого в этом контроллере нужно получить PatientRepository

            IQueryable<VaccinationVM> vaccinations;
            try {
                vaccinations = repository.GetByCondition(v => v.PatientId == patientId);
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                return StatusCode(500);
            }

            Log.Information($"{CurrentMethod.GetName()}: получены все прививки для пациента PatientId = {patientId}");
            return Ok(vaccinations);
        }

        /// <summary>
        /// Чтение прививки по ее Id.
        /// </summary>
        /// <param name="id">Id прививки.</param>
        /// <returns>HTTP ответ содержащий статус код и прививку, или только статус код.</returns>
        /// <response code="200">Возвращает прививку</response>
        /// <response code="404">Ничего не возвращает</response>
        /// <response code="500">Ничего не возвращает</response>
        /// <example>
        /// Пример кода, вызывающего исключение System.ArgumentNullException.
        /// <code>
        /// VaccinationVM vaccination = repository.GetByCondition(null).FirstOrDefault();
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        [HttpGet]
        [Route("vaccinations/{id}")]
        public ActionResult<Vaccination> GetVaccination(int id) {
            VaccinationVM vaccination = null;
            try {
                vaccination = repository.GetByCondition(v => v.Id == id).FirstOrDefault();
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                return StatusCode(500);
            }

            if (vaccination == null) {
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
        /// <returns>HTTP ответ содержащий ответ сервиса.</returns>
        /// <response code="200">Возвращает ответ сервиса</response>
        /// <response code="400">Возвращает ответ сервиса</response>
        /// <response code="500">Возвращает ответ сервиса</response>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        [HttpPost]
        [Route("vaccinations")]
        public ActionResult<ResponseVM> Post([FromBody]VaccinationVM vaccination) {
            if (vaccination == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                ResponseVM response400 = new ResponseVM { IsSuccess = false, ErrorMessage = "Bad request", StatusCode = 400, Result = "Не удалось связать модель, vaccination была равна null" };
                return BadRequest(response400);
            }

            try {
                repository.Add(vaccination);
            }
            catch (DbUpdateConcurrencyException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                //ResponseVM response500 = new ResponseVM { IsSuccess = false, ErrorMessage = "Internal server error", StatusCode = 500, Result = "Внутренняя ошибка сервера" };
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            catch (DbUpdateException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                //ResponseVM response500 = new ResponseVM { IsSuccess = false, ErrorMessage = "Internal server error", StatusCode = 500, Result = "Внутренняя ошибка сервера" };
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }

            Log.Information($"{CurrentMethod.GetName()}: добавлена прививка Id = {vaccination.Id}");
            ResponseVM response200 = new ResponseVM { IsSuccess = true, StatusCode = 200, Result = $"Добавлена прививка Id = {vaccination.Id}" };
            return Ok(response200);
        }

        /// <summary>
        /// Изменение данных прививки.
        /// </summary>
        /// <param name="vaccination">Новые данные прививки.</param>
        /// <param name="id">Id прививки.</param>
        /// <returns>HTTP ответ содержащий ответ сервиса.</returns>
        /// <response code="200">Возвращает ответ сервиса</response>
        /// <response code="400">Возвращает ответ сервиса</response>
        /// <response code="404">Возвращает ответ сервиса</response>
        /// <response code="500">Возвращает ответ сервиса</response>
        /// <example>
        /// Пример кода, вызывающего исключение System.ArgumentNullException.
        /// <code>
        /// VaccinationVM vaccinationFromDB = repository.GetByCondition(null).FirstOrDefault();
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        [HttpPut]
        [Route("vaccinations/{id}")]
        public ActionResult<ResponseVM> Put([FromRoute]int id, [FromBody]VaccinationVM vaccination) {
            if (vaccination == null) {
                Log.Information($"{CurrentMethod.GetName()}: не удалось связать модель");
                ResponseVM response400 = new ResponseVM { IsSuccess = false, ErrorMessage = "Bad request", StatusCode = 400, Result = "Не удалось связать модель, vaccination была равна null" };
                return BadRequest(response400);
            }

            VaccinationVM vaccinationFromDB = null;
            try {
                vaccinationFromDB = repository.GetByCondition(v => v.Id == id).FirstOrDefault();
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                //ResponseVM response500 = new ResponseVM { IsSuccess = false, ErrorMessage = "Internal server error", StatusCode = 500, Result = "Внутренняя ошибка сервера" };
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }

            if (vaccinationFromDB == null) {
                Log.Information($"{CurrentMethod.GetName()}: прививка Id = {id} отсутствует в базе данных");
                ResponseVM response404 = new ResponseVM { IsSuccess = false, ErrorMessage = "Not found", StatusCode = 404, Result = $"Прививка Id = {id} отсутствует в базе данных" };
                return NotFound(response404);
            }

            try {
                repository.Update(vaccination);
            }
            catch (ArgumentNullException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Параметр не может быть равен null\n  Имя параметра: {e.ParamName}\n  Стек вызовов:\n{e.StackTrace}");
                //ResponseVM response500 = new ResponseVM { IsSuccess = false, ErrorMessage = "Internal server error", StatusCode = 500, Result = "Внутренняя ошибка сервера" };
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            catch (DbUpdateConcurrencyException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                //ResponseVM response500 = new ResponseVM { IsSuccess = false, ErrorMessage = "Internal server error", StatusCode = 500, Result = "Внутренняя ошибка сервера" };
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }
            catch (DbUpdateException e) {
                Log.Error($"{CurrentMethod.GetName()}: при обработке запроса произошло исключение {e.GetType()}\n  Метод вызвавший исключение: {e.TargetSite.Name}\n  Стек вызовов:\n{e.StackTrace}");
                //ResponseVM response500 = new ResponseVM { IsSuccess = false, ErrorMessage = "Internal server error", StatusCode = 500, Result = "Внутренняя ошибка сервера" };
                ResponseVM response500 = new ResponseVM(isStatusCode500: true);
                return StatusCode(500, response500);
            }

            Log.Information($"{CurrentMethod.GetName()}: изменена прививка Id = {id}");
            ResponseVM response200 = new ResponseVM { IsSuccess = true, StatusCode = 200, Result = $"Изменена прививка Id = {id}" };
            return Ok(response200);
        }

        /// <summary>
        /// Удаление прививки.
        /// </summary>
        /// <param name="id">Id удаляемой прививки.</param>
        /// <returns>HTTP ответ со статус кодом.</returns>
        /// <response code="200">Ничего не возвращает</response>
        /// <response code="404">Ничего не возвращает</response>
        /// <response code="500">Ничего не возвращает</response>
        /// <exception cref="ArgumentNullException">Происходит когда аргумент равен null.</exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        [HttpDelete]
        [Route("vaccinations/{id}")]
        public IActionResult Delete(int id) {
            VaccinationVM vaccination = repository.GetByCondition(v => v.Id == id).FirstOrDefault();
            if (vaccination == null) {
                Log.Information($"{CurrentMethod.GetName()}: прививка Id = {id} отсутствует в базе данных");
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

            Log.Information($"{CurrentMethod.GetName()}: удалена прививка Id = {id}");
            return Ok();
        }
    }
}
