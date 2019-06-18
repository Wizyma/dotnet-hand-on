using System;
using CrashCourse.Domain.Services;

namespace CrashCourse.Infrastructure.Services
{
    public class ClockService : IClockService
    {
        public DateTime Now => DateTime.Now;
    }
}
