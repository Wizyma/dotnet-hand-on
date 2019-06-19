using System;
using CrashCourse.Domain.Entities;

namespace CrashCourse.Domain.Repositories
{
    public interface IUserRepository
    {
        User Login(string username, string password);
        User GetById(long id);
    }
}
