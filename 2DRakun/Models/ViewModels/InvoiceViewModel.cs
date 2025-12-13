using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Models.ViewModels
{
    public class InvoiceViewModel
    {
        //Kupac
        public int Customer { get; set; }
        public string CustomerOIB { get; set; }
        public string CustomerName { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        //Račun
        public string PdfFilePath { get; set; }
        public int Valuta { get; set; } = 0;
        public string CreatedBy { get; set; }
        public string Cijena_Slovima { get; set; }
        public string Napomena { get; set; }

        public List<InvoiceItemVM> Items { get; set; } = new List<InvoiceItemVM>();
    }

    public class InvoiceItemVM
    {
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? Quantity { get; set; } = 0;
        public decimal? Price { get; set; } = 0;
    }
}