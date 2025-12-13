using _2DRakun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Code
{
    public class InvoiceHelper
    {
        public static decimal CalculateAmount(IEnumerable<InvoiceItem> items)
        {
            if (items == null)
                return 0m;

            return Math.Round(
                items.Sum(i => i.Quantity * i.Price),
                2
            );
        }
    }
}