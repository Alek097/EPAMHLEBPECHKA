using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public class StoreDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }

        public User GetUser(string login)
        {
            return this.Users.FirstOrDefault((usr) => usr.Login == login);
        }
        public void AddUser(User usr)
        {
            this.Users.Add(usr);
            this.SaveChanges();
        }
        public void AddGroup(Group grp)
        {
            this.Groups.Add(grp);
            this.SaveChanges();
        }
        public bool IsContainUser(string login, string pass)
        {
            try
            {
                this.Users.First((u) => u.Login == login && u.Password == pass);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsContainUser(string login)
        {
            try
            {
                this.Users.First((u) => u.Login == login);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ResponceOnQuestion(string login, string response)
        {
            try
            {
                this.Users.First(u => u.Login == login && u.ResponseQuestion == response);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void UpdatePassword(string login, string newPassword)
        {
            User updateUsr = this.Users.First(u => u.Login == login);
            updateUsr.Password = newPassword;
            this.Entry<User>(updateUsr).State = EntityState.Modified;
            this.SaveChanges();
        }
    }
}
