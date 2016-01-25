using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ChudoPechkaLib.Data
{
    public class StoreDB : IDisposable
    {
        protected SqlConnection _dbConnection;
        public static string ConnectionString { get; set; }
        public StoreDB()
        {
            _dbConnection = new SqlConnection(ConnectionString);
            _dbConnection.Open();
        }
        public void Dispose()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
    }
}
