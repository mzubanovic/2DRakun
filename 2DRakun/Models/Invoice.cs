using System;
using System.Collections.Generic;
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
        public DateTime DueDate => IssueDate.AddDays(5);
        public decimal Amount { get; set; } 
        public string Currency { get; set; } = "EUR";
        public string PdfFilePath { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();


    }

}