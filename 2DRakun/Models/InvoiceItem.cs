using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace _2DRakun.Models
{
    [Table("InvoiceItems")]
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public decimal Amount => Math.Round(Quantity * Price, 2);
        [NotMapped]
        public string Price_print => Price == 0 ? "" : Price.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";
        [NotMapped]
        public string Amount_print => Amount == 0 ? "" : Amount.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";
    }
}