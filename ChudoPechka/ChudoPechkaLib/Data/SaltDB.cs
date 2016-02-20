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
        private bool _IsSavedOrModified;
        private static Random _rnd = new Random();
        public DbSet<Salt> Salts { get; set; }
        public string GetSalts(Guid usr_id)
        {
            if (this.IsContainSalt(usr_id))
            {
                return this.Salts.First(s => s.Id == usr_id).SaltString;
            }
            else
            {
                Salt newSalt = new Salt();
                StringBuilder salt = new StringBuilder(_rnd.Next(20, 100));
                for (int i = 0; i < salt.Capacity; i++)
                    salt.Append((char)_rnd.Next(0, 255));

                newSalt.Id = usr_id;
                newSalt.SaltString = salt.ToString();

                this.Salts.Add(newSalt);
                this._IsSavedOrModified = true;

                return newSalt.SaltString;
            }
            
        }

        private bool IsContainSalt(Guid id)
        {
            try
            {
                this.Salts.First(s => s.Id == id);
                return true;
            }
            catch(InvalidOperationException)
            {
                return false;
            }
        }

        public override int SaveChanges()
        {
            if (_IsSavedOrModified)
                return base.SaveChanges();
            else
                return 0;
        }
    }
}
