using System;
namespace CrashCourse.Domain.Exceptions
{
    public class CrashCourseDomainAlreadyClosedException : Exception
    {
        public CrashCourseDomainAlreadyClosedException(long id, DateTime? closedAt)
            :base($"The Crash Course id:{id} was already closed at {closedAt.ToString()}")
        {}
    }
}
