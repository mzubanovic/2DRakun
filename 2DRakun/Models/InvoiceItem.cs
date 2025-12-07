using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; } 
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Price_print { get; set; }
    }
}