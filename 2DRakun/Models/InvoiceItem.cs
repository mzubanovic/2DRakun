using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace _2DRakun.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount => Quantity * Price;
        [NotMapped]
        public string Price_print => Price.ToString("F2", CultureInfo.GetCultureInfo("hr-HR")) + " €";
        [NotMapped]
        public string Amount_print => Amount.ToString("F2", CultureInfo.GetCultureInfo("hr-HR")) + " €";
    }
}