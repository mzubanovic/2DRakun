using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace _2DRakun.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } 
        public int UserId { get; set; }      
        public DateTime IssueDate { get; set; }
        [NotMapped]
        public DateTime DueDate => IssueDate.AddDays(15);
        public decimal Amount { get; set; }
        [NotMapped]
        public decimal PDV => Math.Round(Amount * 0.25m, 2);
        [NotMapped]
        public decimal Amount_saPDV => Math.Round(Amount + PDV, 2);
        [NotMapped]
        public string Amount_saPDV_Print => Amount_saPDV.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";
        [NotMapped]
        public string Amount_Print => Amount.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";
        [NotMapped]
        public string Currency { get; set; } = "EUR";
        public string PdfFilePath { get; set; }
        public string Napomena { get; set; }
        [NotMapped]
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

    }

}