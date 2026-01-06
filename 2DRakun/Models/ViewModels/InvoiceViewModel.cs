using _2DRakun.Code;
using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public string AmountTxt { get; set; }

        public DateTime IssueDate { get; set; }

        public string Note { get; set; }

        public List<InvoiceItemVM> Items { get; set; } = new List<InvoiceItemVM>();

        [NotMapped]
        public List<Customer> ExistingCustomers { get; set; } = new List<Customer>();

        [NotMapped]
        public decimal Amount => InvoiceHelper.CalculateAmount(Items);

        [NotMapped]
        public string Amount_Print => Amount == 0 ? "" : Amount.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";

        [NotMapped]
        public string SellerName { get; set; }

        [NotMapped]
        public string SellerOib { get; set; }

        [NotMapped]
        public string SellerAddress { get; set; }

        [NotMapped]
        public string SellerCity { get; set; }

        [NotMapped]
        public string SellerPostal { get; set; }

        [NotMapped]
        public string SellerPhone { get; set; }

        public string QrCodeBase64 { get; set; }
    }

    public class InvoiceItemVM
    {
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        [NotMapped]
        public string Price_Print
        {
            get
            {
                if (Price == null || Price == 0)
                    return "";
                return Price.Value.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + " €";
            }
        }
    }
}