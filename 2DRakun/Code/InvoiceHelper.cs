using _2DRakun.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace _2DRakun.Code
{

    public class InvoiceHelper
    {
        public static int CreateInvoice(Invoice invoice)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return (int)conn.Insert(invoice);
            }
        }

        public static int CreateInvoice(IDbConnection conn, IDbTransaction tran, Invoice invoice)
        {
            return (int)conn.Insert(invoice, tran);
        }

        public static int CreateInvoiceItems(IDbConnection conn, IDbTransaction tran, InvoiceItem item)
        {
            return (int)conn.Insert(item, tran);
        }

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