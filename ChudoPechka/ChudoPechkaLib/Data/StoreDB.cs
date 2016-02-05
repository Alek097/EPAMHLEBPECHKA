using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public class StoreDB : DbContext, IStoreDB
    {
        private DbSet<User> Users { get; set; }
        private DbSet<Group> Groups { get; set; }
        private DbSet<Order> Orders { get; set; }

        public bool HasUser(string login)
        {
            return this.Users.FirstOrDefault((usr) => usr.Login == login) != null ? true : false;
        }

        public User GetUser(string login)
        {
            return this.Users.FirstOrDefault((usr) => usr.Login == login);
        }
        public void AddUser(User usr)
        {
            this.Users.Add(usr);
            this.SaveChanges();
        }
    }
}
