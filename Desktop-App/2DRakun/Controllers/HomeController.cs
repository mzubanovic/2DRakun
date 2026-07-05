using _2DRakun.Code;
using _2DRakun.Code._2DBarCode;
using _2DRakun.Helpers;
using _2DRakun.Models;
using _2DRakun.Models.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
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
        public ActionResult PreviewInvoice(InvoiceViewModel model)
        {
            if (!ModelState.IsValid || model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "Predračun mora sadržavati barem jednu stavku.");
                return View("NewInvoice", model);
            }

            var userid = AuthHelper.GetCurrentUserId(HttpContext);
            if (userid == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            var user = UsersHelper.GetUserById(userid);

            model.IssueDate = DateTime.Now;

            model.SellerName = (string.IsNullOrEmpty(user.CompanyName) ? user.FirstName + " " + user.LastName : user.CompanyName);
            model.SellerAddress = user.Street;
            model.SellerPostal = user.PostalCode;
            model.SellerCity = user.City;
            model.SellerOib = user.Oib;
            model.SellerIBAN = user.IBAN;
            model.SellerLogoPath = user.LogoPath;

            var items = model.Items.Select(i => new InvoiceItem
            {
                Description = i.Description,
                Unit = i.Unit,
                Quantity = i.Quantity ?? 0,
                Price = i.Price ?? 0m
            }).ToList();
 
            return View("InvoicePreview", model);
        }

      
        public ActionResult ConfirmInvoice(InvoiceViewModel model)
        {

            InvoiceService.ValidateInvoiceModel(model);

            var cUserId = AuthHelper.GetCurrentUserId(HttpContext);

            int customerId = InvoiceService.SaveCustomer(model, cUserId);

            var items = model.Items.Select(i => new InvoiceItem
            {
                Description = i.Description,
                Unit = i.Unit,
                Quantity = i.Quantity ?? 0,
                Price = i.Price ?? 0m
            }).ToList();

            var amount = InvoiceHelper.CalculateAmount(items);

            model.Note += "<br><br>Predračun je izdan u elektroničkom obliku i važeći je bez pečata i potpisa";

            var user = UsersHelper.GetUserById(cUserId);
            if (user == null) {
                return RedirectToAction("Login", "Home");
            }
            model.SellerName = (string.IsNullOrEmpty(user.CompanyName) ? user.FirstName + " " + user.LastName : user.CompanyName);
            model.SellerAddress = user.Street;
            model.SellerPostal = user.PostalCode;
            model.SellerCity = user.City;
            model.SellerOib = user.Oib;
            model.SellerIBAN = user.IBAN;

            if (!string.IsNullOrEmpty(user.LogoPath))
            {
                var physicalPath = Server.MapPath(user.LogoPath);
                if (System.IO.File.Exists(physicalPath))
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(physicalPath);
                    string base64String = Convert.ToBase64String(imageBytes);
                    string imageMimeType = MimeMapping.GetMimeMapping(physicalPath);
                    model.SellerLogoDataUri = $"data:{imageMimeType};base64,{base64String}";
                }
            }

            InvoiceService.AddPdf417BarcodeToModel(model, amount, model.InvoiceNumber, user);

            //Renderiraj view u HTML string
            var htmlContent = PdfHelper.RenderViewToString(
                    ControllerContext,
                    "InvoiceTemplate",
                    model);

            //Generiraj PDF iz HTML stringa
            var pdfBytes = PdfHelper.GeneratePdfFromHtml(htmlContent);

            //Spremi PDF na disk ili vrati kao FileResult
            var invoiceName = $"Invoice_{model.InvoiceNumber}_{DateTime.Now.ToString("dd-MM-yyyy")}.pdf";
            string pdfPath = Server.MapPath($"~/Documents/Invoices/{invoiceName}");
            System.IO.File.WriteAllBytes(pdfPath, pdfBytes);

            model.PdfFilePath = Url.Content($"~/Documents/Invoices/{invoiceName}");

            var invoice = new Invoice
            {
                CustomerId = customerId,
                InvoiceNumber = model.InvoiceNumber,
                UserId = cUserId,
                IssueDate = DateTime.Now,
                PdfFilePath = model.PdfFilePath,
                Note = model.Note,
                AmountTxt = model.AmountTxt,
                Amount = model.Amount
               
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

            TempData["InvoiceModel"] = model;
            return RedirectToAction("Message");
        }

        public ActionResult Message()
        {
            var model = TempData["InvoiceModel"] as InvoiceViewModel;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel model, HttpPostedFileBase logoFile)
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
                Street = model.Street,
                City = model.City,
                PostalCode = model.PostalCode,
                Oib = model.Oib,
                BankName = model.BankName,
                IBAN = model.IBAN,
                Email = model.Email,
                Username = model.Username,
                DateCreated = DateTime.Now,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            if (logoFile != null && logoFile.ContentLength > 0)
            {
                // Validation
                var maxFileSize = 2 * 1024 * 1024; // 2MB
                if (logoFile.ContentLength > maxFileSize)
                {
                    ModelState.AddModelError("logoFile", "Datoteka je veća od 2MB.");
                    return View(model);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(logoFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("logoFile", "Dozvoljeni formati su .jpg, .jpeg, .png.");
                    return View(model);
                }

                // Save file
                var directory = Server.MapPath("~/_LogoSlike");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var path = Path.Combine(directory, fileName);
                logoFile.SaveAs(path);

                user.LogoPath = "/_LogoSlike/" + fileName;
            }


            int newUserId = UsersHelper.CreateUser(user);

            if (newUserId == 0)
            {
                ModelState.AddModelError("", "Pogreška pri registraciji.");
                return View(model);
            }

            Session["UserId"] = newUserId;
            Session["Username"] = user.Username;

            return RedirectToAction("NewInvoice", "Home");
        }

    }
}