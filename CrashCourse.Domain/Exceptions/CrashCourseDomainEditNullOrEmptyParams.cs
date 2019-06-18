using System;
namespace CrashCourse.Domain.Exceptions
{
    public class CrashCourseDomainEditNullOrEmptyParams : Exception
    {
        public CrashCourseDomainEditNullOrEmptyParams(long id, string title, string description)
             : base($"The CrashCourse id:{id} cannot be edited with empty values, expected title and description to not be null or empty, instead got title:{title} and description:{description} id:{id}")
        { }
    }
}
