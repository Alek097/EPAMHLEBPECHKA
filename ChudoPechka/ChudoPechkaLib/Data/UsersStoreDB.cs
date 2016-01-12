using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ChudoPechkaLib.Data
{
    public static class UsersStoreDB
    {
        private static SqlConnection _dbConnection;

        public static void Open(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
            _dbConnection.Open();
        }

        public static void Close()
        {
            _dbConnection.Close();
        }
    }
}
