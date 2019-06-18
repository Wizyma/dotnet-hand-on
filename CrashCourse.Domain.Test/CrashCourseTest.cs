using System;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Exceptions;
using CrashCourse.Domain.Services;
using Moq;
using Xunit;

namespace CrashCourse.Domain.Test
{
    public class CrashCourseTest
    {
        private IClockService GetClockService()
        {
            Mock<IClockService> clockServiceFactory = new Mock<IClockService>();
            clockServiceFactory
                .SetupGet(i => i.Now)
                    .Returns(new DateTime(2019, 01, 1));
            IClockService clockServiceMock = clockServiceFactory.Object;

            return clockServiceMock;
        }

        [Fact]
        public void Test_open_solution()
        {
            var crashCourse = new CrashCourseDomain(1, "Test Set", "Something to test", DateTime.Now);

            var now = new DateTime(2019, 01, 1);
            var solution = "Test case solution";

            var clockService = GetClockService();

            crashCourse.Close(clockService, solution);

            Assert.Equal(crashCourse.ClosedAt, now);
            Assert.Equal(crashCourse.Solution, solution);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Test_error_solution(string solution)
        {
            var crashCourse = new CrashCourseDomain(1, "Test Set", "Something to test", DateTime.Now);

            var clockService = GetClockService();
            Assert.Throws<CrashCourseDomainSolutionException>(() => crashCourse.Close(clockService, solution));
        }

        [Fact]
        public void Test_already_closed()
        {
            var crashCourse = new CrashCourseDomain(1, "Test Set", "Something to test", DateTime.Now);

            var now = new DateTime(2019, 01, 1);
            var solution = "Test case solution";

            var clockService = GetClockService();

            crashCourse.Close(clockService, solution);
            Assert.Throws<CrashCourseDomainAlreadyClosedException>(() => crashCourse.Close(clockService, solution));
        }
    }
}
