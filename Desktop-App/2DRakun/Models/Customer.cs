using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Models
{
    [Table("Customers")]
    public class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; }      
        public string Name { get; set; }     
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Oib { get; set; }
    }
}