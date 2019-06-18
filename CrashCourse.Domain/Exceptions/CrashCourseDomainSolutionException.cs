using System;
namespace CrashCourse.Domain.Exceptions
{
    public class CrashCourseDomainSolutionException : Exception
    {
        public CrashCourseDomainSolutionException(long id)
            :base($"Given the open CrashCourse id:{id}, when you close it, solution can nott be null or empty")
        {}
    }
}
