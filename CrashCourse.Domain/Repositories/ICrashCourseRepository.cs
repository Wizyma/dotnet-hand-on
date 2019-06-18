using System;
using System.Collections;
using System.Collections.Generic;
using CrashCourse.Domain.Entities;

namespace CrashCourse.Domain.Repositories
{
    public interface ICrashCourseRepository
    {
        IEnumerable<CrashCourseDomain> GetAll();
        CrashCourseDomain GetById(long id);
        CrashCourseDomain Add(CrashCourseDomain crashCourse);
        CrashCourseDomain Save(CrashCourseDomain crashCourse);
    }
}
