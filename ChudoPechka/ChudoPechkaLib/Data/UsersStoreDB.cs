using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using ChudoPechkaLib.Data.Model;

namespace ChudoPechkaLib.Data
{
    public static class UsersStoreDB
    {
        private static SqlConnection _dbConnection;

        public static void Add_User(string login, string password, string firstName, string secondName, string secretQuestion, string responseQuestion, DateTime birthDay)
        {
            string sqlExpression = "INSERT INTO Users (Login, Password, FirstName, SecondName, Birthday, SecretQuestion, ResponseQuestion) " +
                string.Format("VALUES (" + @"'{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}'" + ")", login, password, firstName, secondName, birthDay.ToString("u").Replace(" 00:00:00Z", ""), secretQuestion, responseQuestion);
            SqlCommand comAdd = new SqlCommand(sqlExpression, _dbConnection);
            comAdd.ExecuteNonQuery();
        }
        public static bool HasUser(string login)
        {
            string sqlExpression = string.Format("SELECT Login FROM Users WHERE Login = '{0}'", login);
            SqlCommand comSerch = new SqlCommand(sqlExpression, _dbConnection);
            SqlDataReader reader = comSerch.ExecuteReader();
            return reader.HasRows;
        }

        internal static CarrierUser Get_User()
        {
            CarrierUser user = new CarrierUser();

            string sqlExpression = "";

            return user;

        }

        public static void Open(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
            _dbConnection.Open();
        }

        public static void Close()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
    }
}
