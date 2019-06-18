using System;
namespace CrashCourse.Domain.Services
{
    public interface IClockService
    {
        DateTime Now { get; }
    }
}
