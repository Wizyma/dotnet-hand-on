using System;
using CrashCourse.Domain.Exceptions;
using CrashCourse.Domain.Services;

namespace CrashCourse.Domain.Entities
{
    public class CrashCourseDomain
    {
        public long Id { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Solution { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }

        public CrashCourseDomain(long id, string title, string description, DateTime createdAt) :this(id, title, description, createdAt, null, null) { }

        public CrashCourseDomain(long id, string title, string description, DateTime createAt, string solution, DateTime? closedAt)
        {
            Id = id;

            Title = title;
            Description = description;
            Solution = solution;

            CreatedAt = createAt;
            ClosedAt = closedAt;

        }

        public void Close(IClockService clockService, string solution)
        {
            if(string.IsNullOrEmpty(solution?.Trim()))
                throw new CrashCourseDomainSolutionException(Id);

            if (ClosedAt.HasValue)
                throw new CrashCourseDomainAlreadyClosedException(Id, ClosedAt);

            ClosedAt = clockService.Now;
            Solution = solution;
        }

        public void Edit(long id, string title, string description)
        {
            if (string.IsNullOrEmpty(title?.Trim()) || string.IsNullOrEmpty(description?.Trim()))
                throw new CrashCourseDomainEditNullOrEmptyParams(Id, title, description);

            if (ClosedAt.HasValue)
                throw new CrashCourseDomainAlreadyClosedException(Id, ClosedAt);

            Title = title;
            Description = description;

        }

    }
}
