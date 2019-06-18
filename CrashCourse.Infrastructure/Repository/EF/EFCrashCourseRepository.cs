using System;
using System.Collections.Generic;
using System.Linq;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;
using CrashCourse.Domain.Services;
using CrashCourse.Infrastructure.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrashCourse.Infrastructure.Repository.EF
{
    public class EFCrashCourseRepository : ICrashCourseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IClockService _clockService;

        public EFCrashCourseRepository(IConfiguration configuration, IClockService clockService)
        {
            _configuration = configuration;
            _clockService = clockService;
        }

        public CrashCourseDomain Add(string title, string description)
        {
            var model = new CrashCourseModel
            {
                Title = title,
                Description = description,
                CreatedAt = _clockService.Now
            };

            // At the end of the bracket DOTNET will 
            // dispose the connection automaticly
            using (var dbContext = new CrashCourseDBContext(_configuration.GetConnectionString("cnx")))
            {
                dbContext.Database.EnsureCreated();

                dbContext.Add(model);

                dbContext.SaveChanges();
            }

            return model.To();
        }

        public IEnumerable<CrashCourseDomain> GetAll()
        {
            using(var dbContext = new CrashCourseDBContext(_configuration.GetConnectionString("cnx")))
            {
                dbContext.Database.EnsureCreated();

                return dbContext.CrashCourses.ToList().Select(x => x.To());
            }
        }

        public CrashCourseDomain GetById(long id)
        {
             using(var dbContext = new CrashCourseDBContext(_configuration.GetConnectionString("cnx")))
            {
                dbContext.Database.EnsureCreated();

                return dbContext.CrashCourses.Find(id).To();
            }
        }

        public CrashCourseDomain Save(CrashCourseDomain crashCourse)
        {
            var model = CrashCourseModel.From(crashCourse);

            using (var dbContext = new CrashCourseDBContext(_configuration.GetConnectionString("cnx")))
            {
                dbContext.Database.EnsureCreated();

                dbContext.Entry(model).State = EntityState.Modified;

                dbContext.Update(model);

                dbContext.SaveChanges();
            }

            return model.To();
        }
    }
}
