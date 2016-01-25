using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using ChudoPechkaLib.Data.Model;

namespace ChudoPechkaLib.Data
{
    public class GroupsStoreDB : StoreDB
    {
        public void Add_Group(string name, Guid author_id)
        {
            string sqlExpression = "INSERT INTO Groups (Name, author_id) VALUES (@Name, @author_id)";
            SqlCommand comAdd = new SqlCommand(sqlExpression, base._dbConnection);
            comAdd.Parameters.AddWithValue("@Name", name);
            comAdd.Parameters.AddWithValue("@author_id", author_id);
            comAdd.ExecuteNonQuery();
        }
        public List<User> Get_Members(Guid group_id)
        {
            List<User> usrs = new List<User>();
            string sqlExpression = "SELECT * FROM Users_Groups Where Group_id = @Group_id";
            SqlCommand comGet = new SqlCommand(sqlExpression, base._dbConnection);
            comGet.Parameters.AddWithValue("@Group_id", group_id);
            SqlDataReader reader = comGet.ExecuteReader();

            using (reader)
                if (reader.HasRows)
                    using (UsersStoreDB db = new UsersStoreDB())
                        while (reader.Read())
                    {
                        
                            Guid user_id = (Guid)reader["User_id"];
                        
                            usrs.Add(db.Get_UserNonGroup(user_id));
                    }
            return usrs;
        }
        internal void Get_GroupsForUser(User usr)
        {
            Guid user_id = usr.Id;

            List<Guid> MemberGroups = new List<Guid>();//Учавствует в группах

            string sqlExpression = "SELECT * FROM Groups Where author_id = @author_id";
            SqlCommand comGet = new SqlCommand(sqlExpression, base._dbConnection);
            comGet.Parameters.AddWithValue("@author_id", user_id);
            SqlDataReader reader = comGet.ExecuteReader();

            using (reader)
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        Group grp = new Group();

                        grp.Id = (Guid)reader["Id"];
                        grp.Name = (string)reader["Name"];
                        grp.Author = usr;

                        usr.AuthorGroups.Add(grp);
                    }


            sqlExpression = "SELECT * FROM Users_Groups Where User_id = @User_id";
            comGet = new SqlCommand(sqlExpression, base._dbConnection);
            comGet.Parameters.AddWithValue("@User_id", user_id);
            reader = comGet.ExecuteReader();

            using (reader)
                if (reader.HasRows)
                    while (reader.Read())
                        MemberGroups.Add((Guid)reader["Group_id"]);

            if (MemberGroups.Count > 0)
                foreach (Guid item in MemberGroups)
                {
                    Group grp = this.Get_GroupNoneUsers(item);

                    if (grp != null)
                    {
                        grp.Author = usr;

                        usr.Groups.Add(grp);
                    }
                }
        }
        private Group Get_GroupNoneUsers(Guid group_id)
        {
            Group grp = new Group();

            string sqlExpression = "SELECT * FROM Groups Where Id = @Id";
            SqlCommand comGet = new SqlCommand(sqlExpression, base._dbConnection);
            comGet.Parameters.AddWithValue("@Id", group_id);

            SqlDataReader reader = comGet.ExecuteReader();

            using (reader)
                if (reader.HasRows)
                {
                    reader.Read();
                    grp.Id = (Guid)reader["Id"];
                    grp.Name = (string)reader["Name"];
                    return grp;
                }
                else
                    return null;

        }
    }
}
