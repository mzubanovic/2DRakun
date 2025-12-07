using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        // Obrada login forme
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

        // Opcionalno: logout akcija
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // Početna stranica (za prijavljene korisnike)
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
    }
}