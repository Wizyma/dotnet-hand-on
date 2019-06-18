using System;
using System.ComponentModel.DataAnnotations;
using CrashCourse.Domain.Entities;

namespace CrashCourse.Infrastructure.Repository.Models
{
    public class CrashCourseModel
    {
       [Key]
       public long Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public static CrashCourseModel From(CrashCourseDomain crashCourse)
        {
            return new CrashCourseModel
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
