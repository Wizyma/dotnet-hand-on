using System;
using System.Collections.Generic;
using System.Linq;
using CrashCourse.Domain.Entities;
using CrashCourse.Domain.Repositories;

namespace CrashCourse.Infrastructure.Repository.InMemory
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static List<User> _users = new List<User> { 
            new User(1, "Andre", "Toto"),
            new User(2, "Mickael", "Tata"),
        };

        public User GetById(long id)
        {
            return _users.SingleOrDefault(user => user.Id == id);
        }

        public User Login(string username, string password)
        {
            return _users.SingleOrDefault(user => user.Username == username && user.Password == password);
        }
    }
}
