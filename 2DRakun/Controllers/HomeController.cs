using _2DRakun.Code;
using _2DRakun.Helpers;
using _2DRakun.Models;
using _2DRakun.Models.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Unesite email i lozinku.";
                return View();
            }

            var user = AuthHelper.ValidateUser(email, password);

            if (user == null)
            {
                ViewBag.Error = "Neispravan email ili lozinka.";
                return View();
            }

            AuthHelper.SignIn(HttpContext, user);

            return RedirectToAction("NewInvoice", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();          
            Session.Abandon();         
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            ViewBag.UserName = Session["UserName"];
            return View();
        }

        public ActionResult NewInvoice()
        {
            ViewBag.Message = "Your application description page.";

            var model = new InvoiceViewModel();  
            model.ExistingCustomers = CustomerHelper.GetCustomersForUser(AuthHelper.GetCurrentUserId(HttpContext));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInvoice(InvoiceViewModel model)
        {
            if (!ModelState.IsValid || model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "Račun mora sadržavati barem jednu stavku.");
                return View("NewInvoice", model);
            }

            var cUserId = AuthHelper.GetCurrentUserId(HttpContext);

            var nCustomer = new Customer
            {
                Name = model.CustomerName,
                Email = model.CustomerEmail,
                City = model.CustomerCity,
                Street = model.CustomerStreet,
                PostalCode = model.CustomerPostalCode,
                Oib = model.CustomerOib,
                Phone = model.CustomerPhone,
                UserId = cUserId
            };

            int customerId = CustomerHelper.InsertOrUpdateCustomer(nCustomer);

            var items = model.Items.Select(i => new InvoiceItem
            {
                Description = i.Description,
                Unit = i.Unit,
                Quantity = i.Quantity ?? 0,
                Price = i.Price ?? 0m
            }).ToList();

            var amount = InvoiceHelper.CalculateAmount(items);

            model.Note += "<br><br>Račun je izdan u elektroničkom obliku i važeći je bez pečata i potpisa";

            var invoice = new Invoice
            {
                CustomerId = customerId,
                UserId = cUserId,
                IssueDate = DateTime.Now,
                Amount = amount,
                PdfFilePath = model.PdfFilePath,
                Note = model.Note
            };

            DbHelper.ExecuteInTransaction((conn, tran) =>
            {
                var invoiceId = InvoiceHelper.CreateInvoice(conn, tran, invoice);

                foreach (var item in items)
                {
                    item.InvoiceId = invoiceId;
                    InvoiceHelper.CreateInvoiceItems(conn, tran, item);
                }
            });

            TempData["SuccessMessage"] = "Račun je uspješno spremljen.";
            return RedirectToAction("NewInvoice");
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
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel model)
        {
            // Basic backend checks
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Lozinke se ne podudaraju.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("", "Email je obavezan.");
                return View(model);
            }

            var existingUser = UsersHelper.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email već postoji.");
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyName = model.CompanyName,
                Oib = model.Oib,
                BankName = model.BankName,
                IBAN = model.IBAN,
                Email = model.Email,
                Username = model.Username,
                DateCreated = DateTime.Now,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
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