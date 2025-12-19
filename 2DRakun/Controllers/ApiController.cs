using _2DRakun.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2DRakun.Controllers
{
    public class ApiController : Controller
    {
        public class InvoiceItem
        {
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
        }

        [HttpPost]
        public ActionResult CalculateTotalAmount(List<InvoiceItem> items)
        {
            decimal total = items.Sum(i => i.Quantity * i.Price);
            var totalFormatted = total.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";
            return Json(new { totalAmountFormatted = totalFormatted });
        }
    }

}