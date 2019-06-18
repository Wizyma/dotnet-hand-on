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
        /// Post the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        [HttpPost]
        public ActionResult Post([FromBody]CrashCourseDTO dto)
        {
            var crashCrouse = _repository.Add(dto.To());

            return Created($"api/CrashCourse", crashCrouse);
        }

        [HttpPut("{id}/close")]
        public ActionResult<CrashCourseDTO> Put(long id, [FromBody] CrashCourseDTO dto)
        {
            var crashCourse = _repository.GetById(id);

            if (crashCourse == null)
                return NotFound();

            crashCourse.Close(_clockService, dto.Solution);

            _repository.Save(crashCourse);

            return CrashCourseDTO.From(crashCourse);
        }
    }
}
