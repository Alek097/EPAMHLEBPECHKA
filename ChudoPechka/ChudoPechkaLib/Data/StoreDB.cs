using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using ChudoPechkaLib.Data.Model;
namespace ChudoPechkaLib.Data
{
    class StoreDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
