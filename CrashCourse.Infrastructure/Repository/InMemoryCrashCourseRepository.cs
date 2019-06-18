using System;
using System.Collections.Generic;
using System.Linq;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;
using CrashCourse.Domain.Services;

namespace CrashCourse.Infrastructure.Repository
{
    public class InMemoryCrashCourseRepository : ICrashCourseRepository
    {

        private static List<CrashCourseDomain> _data = new List<CrashCourseDomain>();
        private readonly IClockService clockService;

        public InMemoryCrashCourseRepository(IClockService clockService)
        {
            this.clockService = clockService;
        }

        /// <summary>
        /// Add the specified crashCourse.
        /// </summary>
        /// <param name="crashCourse">Crash course.</param>
        public CrashCourseDomain Add(string title, string description)
        {
            var id = _data.Any() ? _data.Max(cCourse => cCourse.Id) + 1 : 1;
            var crashCourseToAdd = new CrashCourseDomain(id, title, description, clockService.Now);

            _data.Add(crashCourseToAdd);

            return crashCourseToAdd;

        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>The all.</returns>
        public IEnumerable<CrashCourseDomain> GetAll()
        {
            return _data;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public CrashCourseDomain GetById(long id)
        {
            return _data.SingleOrDefault(cCourse => cCourse.Id == id);
        }

        public CrashCourseDomain Save(CrashCourseDomain crashCourse) 
        {
            // we should call the database here
            var localCrashCourse = _data.SingleOrDefault(_c => _c.Id == crashCourse.Id);

            return crashCourse;
        }
    }
}
