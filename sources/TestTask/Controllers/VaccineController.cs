﻿using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using TestTask.Models;
using TestTask.Models.Repository;
using TestTask.Services;

namespace TestTask.Controllers {
    /// <summary>
    /// Контроллер для действий с сущностью "Препарат".
    /// </summary>
    /// <remarks>
    /// Предоставляет метод чтения препаратов.
    /// </remarks>
    [ApiExplorerSettings(GroupName = "vaccine-management")]
    [Route("api/vaccine-management/vaccines")]
    [ApiController]
    public class VaccineController : ControllerBase {
        private readonly IDataRepository<Vaccine> repository;

        public VaccineController(IDataRepository<Vaccine> dataRepository) {
            repository = dataRepository;
        }

        /// <summary>
        /// Чтение всех препаратов.
        /// </summary>
        /// <returns>HTTP ответ содержащий статус код и препараты.</returns>
        /// <response code="200">Возвращает все препараты</response>
        [HttpGet]
        public ActionResult<IEnumerable<Vaccine>> Get() {
            IQueryable<Vaccine> vaccines = repository.GetAll();
            Log.Information($"{CurrentMethod.GetName()}: получены все препараты");
            return Ok(vaccines);
        }
    }
}
