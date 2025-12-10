using _2DRakun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Code
{
    public class AuthHelper
    {
        public static Users ValidateUser(string email, string password)
        {
            var user = UsersHelper.GetUserByEmail(email);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        public static void SignIn(HttpContextBase context, Users user)
        {
            context.Session["UserId"] = user.Id;
            context.Session["UserEmail"] = user.Email;
            context.Session["UserName"] = user.Username;
        }

        public static string GetCurrentUsername(HttpContextBase context)
        {
            return context.Session["UserName"] as string;
        }
    }
}