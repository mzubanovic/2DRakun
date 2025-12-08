using _2DRakun.Models;
using Dapper;
using System;

namespace _2DRakun
{
    public class UsersHelper
    {
        public User GetUserById(int id)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return conn.Get<User>(id);  
            }
        }

        public int UpdateUser(User user)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return conn.Update(user);  
            }
        }

        public static int DeleteUser(int id)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                var user = new User { Id = id };
                return conn.Delete(user);  
            }
        }

        public static int CreateUser(User user)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return (int)conn.Insert(user);  
            }
        }

        /// <summary>
        /// Retrieves a user from the database by the specified email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="User"/> object if a user with the specified email exists; 
        /// otherwise, <c>null</c>.
        /// </returns>
        public static User GetUserByEmail(string email) {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return (User)conn.QueryFirstOrDefault("SELECT top(1) * FROM USERS WHERE email = @email" , new { email});
            }
        }
    }
}
