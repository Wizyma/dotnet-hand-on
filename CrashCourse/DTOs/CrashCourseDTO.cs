using System;
using CrashCourse.Domain.Entities;

namespace CrashCourse.DTOs
{
    public class CrashCourseDTO
    {
        public long Id { get; private set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public static CrashCourseDTO From(CrashCourseDomain crashCourse)
        {
            return new CrashCourseDTO
            {
                Id = crashCourse.Id,
                Title = crashCourse.Title,
                Description = crashCourse.Description,
                Solution = crashCourse.Solution,
                CreatedAt = crashCourse.CreatedAt,
                ClosedAt = crashCourse.ClosedAt
            };
        }


        public CrashCourseDomain To()
        {
            return new CrashCourseDomain(Id, Title, Description, CreatedAt, Solution, ClosedAt);
        }
    }
}
