using System;
using Dapper;
using System.Collections.Generic;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;
using CrashCourse.Domain.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using CrashCourse.Infrastructure.Repository.Models;
using System.Linq;

namespace CrashCourse.Infrastructure.Repository.Dapper
{
    public class DapperCrashCourseRepository : ICrashCourseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IClockService _clockService;
        private const string _tableName = "crashcourse";
        
        public DapperCrashCourseRepository(IConfiguration configuration, IClockService clockService)
        {
            _configuration = configuration;
            _clockService = clockService;
        }

        public CrashCourseDomain Add(string title, string description)
        {
            var model = new CrashCourseModel
            {
                Title = title,
                Description = description,
                CreatedAt = _clockService.Now
            };

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("cnx")))
            {
                connection.Open();

                var row = connection.Execute($"INSERT INTO {_tableName} (title, description, createdAt) Values (@Title, @Description, @CreatedAt);", model); 
            }

            return model.To();
        }

        public IEnumerable<CrashCourseDomain> GetAll()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("cnx")))
            {
                connection.Open();

                return connection.Query<CrashCourseModel>($"SELECT * FROM {_tableName}")
                    .ToList()
                    .Select(x => x.To());
            }

        }

        public CrashCourseDomain GetById(long id)
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("cnx")))
            {
                connection.Open();

                return connection
                    .QuerySingleOrDefault<CrashCourseModel>($"SELECT * FROM {_tableName} WHERE id=@Id", new { Id = id })
                    .To();
            }
        }

        public CrashCourseDomain Save(CrashCourseDomain crashCourse)
        {
            var model = CrashCourseModel.From(crashCourse);

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("cnx")))
            {
                connection.Open();

                var row = connection.Execute($"UPDATE {_tableName} SET description = @Description, title = @Title, solution = @Solution, closedAt = @ClosedAt WHERE id = @Id;", model);
            }

            return model.To();
        }
    }
}
