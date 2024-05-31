using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QandAsite.Data
{
    public class UserRepo
    {
        private readonly string _connectionString;
        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (isValidPassword)
            {
                return user;
            }
            return null;
        }

        public User GetByEmail(string email)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }
        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.Password = hash;
            using var ctx = new QuestionsDataContext(_connectionString);
            ctx.Users.Add(user);
            ctx.SaveChanges();

        }
      
    }
}
