using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2DRakun.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Dapper.Required, StringLength(50)]
        public string FirstName { get; set; }

        [Dapper.Required, StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB mora imati 11 znakova")]
        public string Oib { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [StringLength(34)] // Max length for IBAN
        public string IBAN { get; set; }

        [Dapper.Required, EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        public DateTime DateCreated { get; set; }

        [NotMapped]
        public int PaymentTerm => 15; //valuta

        [Dapper.Required]
        public string PasswordHash { get; set; }
    }
}