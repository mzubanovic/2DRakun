using _2DRakun.Models;
using Dapper;
using System;
using System.Web;

namespace _2DRakun
{
    public class UsersHelper
    {
        public Users GetUserById(int id)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return conn.Get<Users>(id);  
            }
        }

        public int UpdateUser(Users user)
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
                var user = new Users { Id = id };
                return conn.Delete(user);  
            }
        }

        public static int CreateUser(Users user)
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
        /// A <see cref="Users"/> object if a user with the specified email exists; 
        /// otherwise, <c>null</c>.
        /// </returns>
        public static Users GetUserByEmail(string email) {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return conn.QueryFirstOrDefault<Users>("SELECT top(1) * FROM USERS WHERE email = @email" , new { email});
            }
        }

    }
}
