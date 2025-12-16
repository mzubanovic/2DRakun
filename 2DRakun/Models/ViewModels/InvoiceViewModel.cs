using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOib { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string PdfFilePath { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }

        public string Note { get; set; }

        public List<InvoiceItemVM> Items { get; set; } = new List<InvoiceItemVM>();
    }

    public class InvoiceItemVM
    {
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; } 
    }
}