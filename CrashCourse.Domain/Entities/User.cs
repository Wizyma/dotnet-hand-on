using System;
namespace CrashCourse.Domain.Entities
{
    public class User
    {
        public long Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User(long id, string username, string password) 
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
