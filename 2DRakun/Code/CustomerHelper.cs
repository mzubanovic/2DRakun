using _2DRakun.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _2DRakun.Helpers
{
    public class CustomerHelper
    {

        public static int UpdateCustomer(Customer customer)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return conn.Update(customer);
            }
        }

        public static int CreateCustomer(Customer customer)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                return (int)conn.Insert(customer);
            }
        }
        /// <summary>
        /// Insert or update customer within existing DB connection and transaction.
        /// Returns Customer.Id.
        /// </summary>
        public static int InsertOrUpdateCustomer(Customer customer)
        {
            //check if customer already exists in db
            var existingCustomerId = GetExistingCustomerId(customer.Oib, customer.UserId);

            if (existingCustomerId > 0)
            {
                //Customer already exists in db - update with latest data
                customer.Id = existingCustomerId;
                UpdateCustomer(customer);
                return existingCustomerId;
            }
            else
            {
                //Customer doesnt exist in db - insert new customer
                return CreateCustomer(customer);
            }
        }

        public static int GetExistingCustomerId(string oib, int userId)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                var sql = "SELECT Id FROM Customers WHERE Oib = @Oib AND UserId = @UserId";
                return conn.QueryFirstOrDefault<int>(sql, new { Oib = oib, UserId = userId });
            }
        }

        public static List<Customer> GetCustomersForUser(int userId)
        {
            using (var conn = DbHelper.GetOpenConnection())
            {
                var sql = "SELECT * FROM Customers WHERE UserId = @UserId ORDER BY Name";
                return conn.Query<Customer>(sql, new { UserId = userId }).ToList();
            }
        }
    }
}
