using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;
using CrashCourse.Domain.Services;
using CrashCourse.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrashCourse.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CrashCourseController : Controller
    {
        private readonly ICrashCourseRepository _repository;
        private readonly IClockService _clockService;
        private readonly IUserRepository _userRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrashCourse.Api.Controllers.CrashCourseController"/> class.
        /// </summary>
        /// <param name="repository">Repository.</param>
        /// <param name="clockService">Clock service.</param>
        public CrashCourseController(ICrashCourseRepository repository, IClockService clockService, IUserRepository userRepository)
        {
            _repository = repository;
            _clockService = clockService;
            _userRepository = userRepository;
        }

        private string GenerateToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("userid", user.Id.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FLDKJSKFDDKFJHKDJFIOENZBKBDFSOHFJDFBKNCIODHFOUABEZFJKLSDFJK")), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);

            return token;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<string> Login([FromBody] LoginDTO dto)
        {
            User user = _userRepository.Login(dto.username, dto.password);

            if (user == null)
                return NotFound();

            return GenerateToken(user);
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

            return CrashCourseDTO.From(_repository.Save(crashCourse));
        }

        [HttpPatch("{id}/edit")]
        public ActionResult<CrashCourseDTO> Edit(long id, [FromBody] JsonPatchDocument<EditCrashCourseDTO> crashCoursePatch)
        {
            var crashCourse = _repository.GetById(id);

            if (crashCourse == null)
                return NotFound();

            var dto = new EditCrashCourseDTO
            {
                Title = crashCourse.Title,
                Description = crashCourse.Description
            };

            crashCoursePatch.ApplyTo(dto);

            crashCourse.Edit(id, dto.Title, dto.Description);


            return CrashCourseDTO.From(_repository.Save(crashCourse));

        }
    }
}
