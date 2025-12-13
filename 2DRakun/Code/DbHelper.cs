using System;
using System.Configuration;
using System.Data.SqlClient;

namespace _2DRakun
{

    public static class DbHelper
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["connsString"].ConnectionString;

        public static string GetConnectionString()
        {
            return _connectionString;
        }

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public static void ExecuteInTransaction(Action<SqlConnection, SqlTransaction> action)
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

        public static void Execute(Action<SqlConnection> action)
        {
            using (var conn = GetOpenConnection())
            {
                action(conn);
            }
        }
    }

}