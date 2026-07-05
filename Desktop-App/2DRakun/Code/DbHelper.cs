using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Configuration;
using System.IO;

namespace _2DRakun
{
    public static class DbHelper
    {
        private static readonly string _dbPath;
        private static readonly string _connectionString;

        static DbHelper()
        {
            string connString = ConfigurationManager.ConnectionStrings["connsString"].ConnectionString;
            
            // Resolve |DataDirectory| for desktop application
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (string.IsNullOrEmpty(dataDirectory))
            {
                dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "2DRakun");
                Directory.CreateDirectory(dataDirectory);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
            }

            _dbPath = connString.Replace("|DataDirectory|", dataDirectory);
            _connectionString = $"Data Source={_dbPath}";
        }

        public static void InitializeDatabase()
        {
            if (File.Exists(_dbPath))
            {
                return;
            }

            using (var conn = GetOpenConnection())
            {
                // Create tables
                conn.Execute(@"
                    CREATE TABLE Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT,
                        LastName TEXT,
                        CompanyName TEXT,
                        Street TEXT,
                        City TEXT,
                        PostalCode TEXT,
                        Oib TEXT,
                        BankName TEXT,
                        IBAN TEXT,
                        Email TEXT,
                        Username TEXT,
                        DateCreated TEXT,
                        PasswordHash TEXT,
                        LogoPath TEXT
                    );
                ");

                conn.Execute(@"
                    CREATE TABLE Customers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER,
                        Name TEXT,
                        Street TEXT,
                        City TEXT,
                        PostalCode TEXT,
                        Email TEXT,
                        Phone TEXT,
                        Oib TEXT
                    );
                ");

                conn.Execute(@"
                    CREATE TABLE Invoices (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER,
                        CustomerName TEXT,
                        CustomerStreet TEXT,
                        CustomerCity TEXT,
                        CustomerPostalCode TEXT,
                        CustomerEmail TEXT,
                        CustomerPhone TEXT,
                        CustomerOib TEXT,
                        InvoiceNumber TEXT,
                        UserId INTEGER,
                        IssueDate TEXT,
                        Amount REAL,
                        AmountTxt TEXT,
                        PdfFilePath TEXT,
                        Note TEXT
                    );
                ");

                conn.Execute(@"
                    CREATE TABLE InvoiceItems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        InvoiceId INTEGER,
                        Description TEXT,
                        Unit TEXT,
                        Quantity REAL,
                        Price REAL
                    );
                ");
            }
        }

        public static string GetConnectionString()
        {
            return _connectionString;
        }

        public static SqliteConnection GetOpenConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public static void ExecuteInTransaction(Action<SqliteConnection, SqliteTransaction> action)
        {
            using (var conn = GetOpenConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    action(conn, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public static void Execute(Action<SqliteConnection> action)
        {
            using (var conn = GetOpenConnection())
            {
                action(conn);
            }
        }
    }
}