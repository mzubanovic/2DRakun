using _2DRakun.Models;
using _2DRakun.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace _2DRakun.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            //validacija
            //logiranje

            return View();
        }

        private bool VerifyPassword(string plainPassword, string storedHash)
        {
            // Za primjer, jednostavna provjera NE KORISTITI
            return plainPassword == storedHash;
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            ViewBag.UserName = Session["UserName"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = UsersHelper.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email već postoji.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                DateCreated = DateTime.Now
            };

            int newUserId = UsersHelper.CreateUser(user);

            if (newUserId == 0)
            {
                ModelState.AddModelError("", "Pogreška pri registraciji.");
                return View(model);
            }

            Session["UserId"] = newUserId;
            Session["Username"] = user.Username;

            return RedirectToAction("Index", "Home");
        }

    }
}