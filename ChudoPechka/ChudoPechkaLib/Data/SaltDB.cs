using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public class SaltDB : DbContext
    {
        private static Random _rnd = new Random();
        public DbSet<Salt> Salts { get; set; }
        public string GetSalt(Guid usr_id)
        {
            if (this.IsContainSalt(usr_id))
            {
                return this.Salts.First(s => s.Id == usr_id).SaltString;
            }
            else
            {
                return this.AddSalt(usr_id);
            }

        }
        public string UpdateSalt(Guid id)
        {
            if (this.IsContainSalt(id))
            {
                Salt upSalt = this.Salts.First(s => s.Id == id);
                upSalt.SaltString = this.GenerateSalt();
                this.Entry<Salt>(upSalt).State = EntityState.Modified;
                SaveChanges();

                return upSalt.SaltString;
            }
            else
            {
                return this.AddSalt(id);
            }
        }

        private bool IsContainSalt(Guid id)
        {
            try
            {
                this.Salts.First(s => s.Id == id);
                return true;
            }
            catch(InvalidCastException)
            {
                return false;
            }
        }
        private string GenerateSalt()
        {
            StringBuilder salt = new StringBuilder(_rnd.Next(20, 100));
            for (int i = 0; i < salt.Capacity; i++)
                salt.Append((char)_rnd.Next(-128, 127));
            return salt.ToString();
        }
        private string AddSalt(Guid id)
        {
            Salt newSalt = new Salt();
            newSalt.Id = id;
            newSalt.SaltString = this.GenerateSalt();

            this.Salts.Add(newSalt);
            SaveChanges();

            return newSalt.SaltString;
        }
    }
}
