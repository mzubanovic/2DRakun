using System.Configuration;
using System.Data.SqlClient;

namespace _2DRakun.Code
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
    }

}