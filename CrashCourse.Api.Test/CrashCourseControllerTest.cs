using System;
using System.Linq;
using CrashCourse.Api.Controllers;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;
using CrashCourse.Domain.Services;
using CrashCourse.DTOs;
using Moq;
using NFluent;
using Xunit;

namespace CrashCourse.Api.Test
{
    public class CrashCourseControllerTest
    {
        private Mock<IUserRepository> _userRepository;

        [Fact]
        public void Test_crash_course_controller_get_all()
        {
            var mockData = new[] { 
                new CrashCourseDomain(1, "Controller Test", "Description Controller Test", DateTime.Now) 
            };

            var crashCourseRepositoryFactory = new Mock<ICrashCourseRepository>();
            crashCourseRepositoryFactory
                .Setup(i => i.GetAll())
                .Returns(mockData);

            var crashCourseRepositoryMock = crashCourseRepositoryFactory.Object;

            var clockServiceFactory = new Mock<IClockService>();
            var clockServiceMock = clockServiceFactory.Object;

            var controller = new CrashCourseController(crashCourseRepositoryMock, clockServiceMock, _userRepository.Object);

            var result = controller.Get();
            Assert.Equal(result.Count(), 1);
            // Check.That(result).Equals(mockData.Select(CrashCourseDTO.From));
        }

        [Fact]
        public void Test_crash_course_controller_get_by_id()
        {
            var mockData = new[] {
                new CrashCourseDomain(1, "Controller Test", "Description Controller Test", DateTime.Now)
            };

            var crashCourseRepositoryFactory = new Mock<ICrashCourseRepository>();
            crashCourseRepositoryFactory
                .Setup(i => i.GetAll())
                .Returns(mockData);

            var crashCourseRepositoryMock = crashCourseRepositoryFactory.Object;

            var clockServiceFactory = new Mock<IClockService>();
            var clockServiceMock = clockServiceFactory.Object;

            var controller = new CrashCourseController(crashCourseRepositoryMock, clockServiceMock, _userRepository.Object);

            var result = controller.GetById(1);

            crashCourseRepositoryFactory.Verify(r => r.GetById(1), "Not invoked");
            // Check.That(result).Equals(mockData.Select(CrashCourseDTO.From));
        }

        [Fact]
        public void Test_crash_course_controller_edit()
        {
            var mockData = new[] {
                new CrashCourseDomain(1, "Controller Test", "Description Controller Test", DateTime.Now)
            };

            var crashCourseRepositoryFactory = new Mock<ICrashCourseRepository>();
            crashCourseRepositoryFactory
                .Setup(i => i.GetById(It.IsAny<long>()))
                .Returns(mockData[0]);

            crashCourseRepositoryFactory
                .Setup(i => i.Save(It.IsAny<CrashCourseDomain>()))
                .Returns(mockData[0]);

            var crashCourseRepositoryMock = crashCourseRepositoryFactory.Object;

            var clockServiceFactory = new Mock<IClockService>();
            var clockServiceMock = clockServiceFactory.Object;

            var controller = new CrashCourseController(crashCourseRepositoryMock, clockServiceMock, _userRepository.Object);

            var result = controller.Edit(1, new EditCrashCourseDTO() { Title = "Test", Description = "Test Desc" });

            crashCourseRepositoryFactory.Verify(r => r.GetById(1), "Not invoked");
            crashCourseRepositoryFactory.Verify(r => r.Save(It.IsAny<CrashCourseDomain>()), "Not invoked");
        }

        [Fact]
        public void Test_crash_course_controller_close()
        {
            var mockData = new[] {
                new CrashCourseDomain(1, "Controller Test", "Description Controller Test", DateTime.Now)
            };

            var crashCourseRepositoryFactory = new Mock<ICrashCourseRepository>();
            crashCourseRepositoryFactory
                .Setup(i => i.GetById(It.IsAny<long>()))
                .Returns(mockData[0]);

            crashCourseRepositoryFactory
                .Setup(i => i.Save(It.IsAny<CrashCourseDomain>()))
                .Returns(mockData[0]);

            var crashCourseRepositoryMock = crashCourseRepositoryFactory.Object;

            var clockServiceFactory = new Mock<IClockService>();
            var clockServiceMock = clockServiceFactory.Object;

            var controller = new CrashCourseController(crashCourseRepositoryMock, clockServiceMock, _userRepository.Object);

            var result = controller.Put(1, new CloseCrashCourseDTO() { Solution = "Test close" });

            crashCourseRepositoryFactory.Verify(r => r.GetById(1), "Not invoked");
            crashCourseRepositoryFactory.Verify(r => r.Save(It.IsAny<CrashCourseDomain>()), "Not invoked");

            Assert.NotNull(result.Value.ClosedAt);
        }
    }
}
