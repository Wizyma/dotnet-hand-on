using System;
using System.Reflection;
using CrashCourse.Infrastructure.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CrashCourse.Infrastructure.Repository.EF
{
    public class CrashCourseDBContext : DbContext
    {
        private readonly string _connectionString;

        public CrashCourseDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<CrashCourseModel> CrashCourses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrashCourseModel>().ToTable("crashcourse", "crashcourseschema");

            base.OnModelCreating(modelBuilder);
        }
    }
}
