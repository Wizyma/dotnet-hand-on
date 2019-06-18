using System.Collections.Generic;
using System.Linq;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;
using CrashCourse.Domain.Services;
using CrashCourse.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrashCourse.Api.Controllers
{
    [Route("api/[controller]")]
    public class CrashCourseController : Controller
    {
        private readonly ICrashCourseRepository _repository;
        private readonly IClockService _clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrashCourse.Api.Controllers.CrashCourseController"/> class.
        /// </summary>
        /// <param name="repository">Repository.</param>
        /// <param name="clockService">Clock service.</param>
        public CrashCourseController(ICrashCourseRepository repository, IClockService clockService)
        {
            _repository = repository;
            _clockService = clockService;
        }

        /// <summary>
        /// Get this instance.
        /// </summary>
        /// <returns>The get.</returns>
        [HttpGet]
        public IEnumerable<CrashCourseDTO> Get()
        {
            return _repository
                .GetAll()
                .Select(CrashCourseDTO.From);
        }


        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        [HttpGet("{id}")]
        public ActionResult<CrashCourseDTO> GetById(long id)
        {
            var crashCourse = _repository.GetById(id);

            if (crashCourse == null) return NotFound();

            return CrashCourseDTO.From(crashCourse);
        }

        /// <summary>
        /// Post the specified dto.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="dto">Dto.</param>
        [HttpPost]
        public ActionResult Post([FromBody]AddCrashCourseDTO dto)
        {
            var crashCrouse = _repository.Add(dto.Title, dto.Description);

            return Created($"api/CrashCourse", crashCrouse);
        }

        /// <summary>
        /// Put the specified id and dto.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="dto">Dto.</param>
        [HttpPut("{id}/close")]
        public ActionResult<CrashCourseDTO> Put(long id, [FromBody] CloseCrashCourseDTO dto)
        {
            var crashCourse = _repository.GetById(id);

            if (crashCourse == null)
                return NotFound();

            crashCourse.Close(_clockService, dto.Solution);

            _repository.Save(crashCourse);

            return CrashCourseDTO.From(crashCourse);
        }

        [HttpPut("{id}/edit")]
        public ActionResult<CrashCourseDTO> Edit(long id, [FromBody] EditCrashCourseDTO dto)
        {
            var crashCourse = _repository.GetById(id);

            if (crashCourse == null)
                return NotFound();

            crashCourse.Edit(id, dto.Title, dto.Description);

            _repository.Save(crashCourse);

            return CrashCourseDTO.From(crashCourse);
        }
    }
}
