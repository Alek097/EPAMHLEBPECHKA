using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using ChudoPechkaLib.Data.Model;

namespace ChudoPechkaLib.Data
{
    public  class UsersStoreDB : StoreDB
    {
        

        public void Add_User(string login, string password, string firstName, string secondName, string secretQuestion, string responseQuestion, DateTime birthDay)
        {
            string sqlExpression = "INSERT INTO Users (Login, Password, FirstName, SecondName, Birthday, SecretQuestion, ResponseQuestion, AvatarPath) VALUES (@Login, @Password, @FirstName, @SecondName, @BirthDay, @SecretQuestion, @ResonseQuestion, @AvatarPath)";
            SqlCommand comAdd = new SqlCommand(sqlExpression, base._dbConnection);
            comAdd.Parameters.AddWithValue("@Login", login);
            comAdd.Parameters.AddWithValue("@Password", password);
            comAdd.Parameters.AddWithValue("@FirstName", firstName);
            comAdd.Parameters.AddWithValue("@SecondName", secondName);
            comAdd.Parameters.AddWithValue("@BirthDay", birthDay);
            comAdd.Parameters.AddWithValue("@SecretQuestion", secretQuestion);
            comAdd.Parameters.AddWithValue("@ResonseQuestion", responseQuestion);
            comAdd.ExecuteNonQuery();
        }
        public bool HasUser(string login)
        {
            string sqlExpression = "SELECT Login FROM Users WHERE Login = @Login";
            SqlCommand comSerch = new SqlCommand(sqlExpression, _dbConnection);
            comSerch.Parameters.AddWithValue("@Login", login);

            using (SqlDataReader reader = comSerch.ExecuteReader())
                return reader.HasRows;
        }

        public bool HasUser(string login, string password)
        {
            User user = new User();
            string sqlExpression = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";

            SqlCommand comGet = new SqlCommand(sqlExpression, _dbConnection);
            comGet.Parameters.AddWithValue("@Login", login);
            comGet.Parameters.AddWithValue("@Password", password);
            SqlDataReader reader = comGet.ExecuteReader();

            using (reader)
            {
                return reader.HasRows;
            }

        }
        internal User Get_UserNonGroup(Guid user_id)
        {
            User user = new User();
            string sqlExpression = "SELECT * FROM Users Where Id = @Id";

            SqlCommand comGet = new SqlCommand(sqlExpression, _dbConnection);
            comGet.Parameters.AddWithValue("@Id", user_id);
            SqlDataReader reader = comGet.ExecuteReader();

            using (reader)
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    user.Id = (Guid)reader["id"];
                    user.Login = (string)reader["Login"];
                    user.Password = (string)reader["Password"];
                    user.FirsName = (string)reader["FirstName"];
                    user.SecondName = (string)reader["SecondName"];
                    user.BirthDay = (DateTime)reader["Birthday"];
                    user.SecretQuestion = (string)reader["SecretQuestion"];
                    user.ResponseQuestion = (string)reader["ResponseQuestion"];
                    user.AvatarPath = (string)reader["AvatarPath"];
                }
                else
                    throw new ArgumentException();
            }

            return user;
        }

        public User Get_User(string login)
        {
            User user = new User();
            string sqlExpression = "SELECT * FROM Users Where Login = @Login";

            SqlCommand comGet = new SqlCommand(sqlExpression, _dbConnection);
            comGet.Parameters.AddWithValue("@Login", login);
            SqlDataReader reader = comGet.ExecuteReader();

            using (reader)
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    user.Id = (Guid)reader["id"];
                    user.Login = (string)reader["Login"];
                    user.Password = (string)reader["Password"];
                    user.FirsName = (string)reader["FirstName"];
                    user.SecondName = (string)reader["SecondName"];
                    user.BirthDay = (DateTime)reader["Birthday"];
                    user.SecretQuestion = (string)reader["SecretQuestion"];
                    user.ResponseQuestion = (string)reader["ResponseQuestion"];
                    user.AvatarPath = (string)reader["AvatarPath"];

                    using (GroupsStoreDB Group_DB = new GroupsStoreDB())
                        Group_DB.Get_GroupsForUser(user);
                }
                else
                    throw new ArgumentException();
            }

            return user;

        }

        public bool UpdatePassword(string login, string newPass, string responseQuestion)
        {
            if (!this.HasUser(login))
                throw new NullReferenceException();

            User usr = this.Get_User(login);

            if (usr.ResponseQuestion.Equals(responseQuestion))
            {
                string sqlExpression = "UPDATE Users SET Password=@Password WHERE Login=@Login";
                using (SqlCommand comUpdate = new SqlCommand(sqlExpression, _dbConnection))
                {
                    comUpdate.Parameters.AddWithValue("@Password", newPass);
                    comUpdate.Parameters.AddWithValue("@Login", login);
                    comUpdate.ExecuteNonQuery();
                    return true;
                }
            }
            else
                return false;
        }
    }
}
