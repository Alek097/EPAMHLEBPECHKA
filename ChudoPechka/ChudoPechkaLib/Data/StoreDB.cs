using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Data.Entity;
using System.Collections.Generic;

using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public class StoreDB : DbContext, IStoreDB
    {
        private bool _IsSavedOrModified;
        private SaltDB _saltDB = new SaltDB();
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.AdministartionGroups)
                .WithMany(g => g.Administrations)
                .Map(m => m.ToTable("Admonistration"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Groups)
                .WithMany(g => g.Users)
                .Map(m => m.ToTable("UsersGroups"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Announced> Announceds { get; set; }

        public User GetUser(string login)
        {
            return this.Users
                .Include(u => u.Groups)
                .Include(u => u.Announceds)
                .First((usr) => usr.Login.Equals(login));
        }
        public User GetUser(Guid usr_id)
        {
            return this.Users
                .Include(u => u.Groups)
                .Include(u => u.Announceds)
                .First((usr) => usr.Id.Equals(usr_id));
        }
        public Group GetGroup(Guid group_id)
        {
            return this.Groups
               .Include(g => g.Administrations)
               .Include(g => g.Users)
               .First(g => g.Id.Equals(group_id));
        }
        public void AddUser(User usr)
        {
            string salt = _saltDB.GetSalt(usr.Id);

            usr.Password = this.Encrypt(usr.Password, salt);
            usr.ResponseQuestion = this.Encrypt(usr.ResponseQuestion, salt);

            this.Users.Add(usr);
            this._IsSavedOrModified = true;
        }
        public void AddGroup(Group grp)
        {
            this.Groups.Add(grp);
            this._IsSavedOrModified = true;
        }
        public void AddMemberInGroup(Guid group_id, User usr)
        {
            Group updateGrp = this.GetGroup(group_id);
            if (!updateGrp.Users.Contains(usr))
            {
                updateGrp.Users.Add(usr);
                this.Entry<Group>(updateGrp).State = EntityState.Modified;

                this._IsSavedOrModified = true;
            }
        }
        public void AddAuthorInGroup(Guid group_id, string login)
        {
            Group updateGrp = this.GetGroup(group_id);
            User usr = this.GetUser(login);
            if (updateGrp.Users.Contains(usr))
            {
                updateGrp.Users.Remove(usr);
                updateGrp.Administrations.Add(usr);
                this.Entry<Group>(updateGrp).State = EntityState.Modified;

                this._IsSavedOrModified = true;
            }
        }
        public bool IsContainGroup(Guid group_id)
        {
            try
            {
                this.Groups.First((u) => u.Id.Equals(group_id));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool IsContainUser(string login, string pass)
        {
            try
            {
                if (this.IsContainUser(login))
                {
                    User usr = this.GetUser(login);
                    string encryptPass = this.Encrypt(pass, _saltDB.GetSalt(usr.Id));
                    this.Users.First((u) => u.Login.Equals(login) && u.Password.Equals(encryptPass));
                    return true;
                }
                else
                    return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool IsContainUser(string login)
        {
            try
            {
                this.Users.First((u) => u.Login.Equals(login));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool IsContainUser(Guid usr_id)
        {
            try
            {
                this.Users.First((u) => u.Id.Equals(usr_id));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool IsContainAnnounced(Guid From_id)
        {
            try
            {
                this.Announceds.First(a => a.From.Equals(From_id));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool ResponceOnQuestion(string login, string response)
        {

            try
            {
                User usr = this.GetUser(login);
                string responseWithSalt = Encrypt(response, _saltDB.GetSalt(usr.Id));
                this.Users.First(u => u.Login.Equals(login) && u.ResponseQuestion.Equals(responseWithSalt));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void UpdatePassword(string login, string newPassword, string responseQuestion)
        {
            User updateUsr = this.Users.First(u => u.Login.Equals(login));

            string salt = this._saltDB.UpdateSalt(updateUsr.Id);

            updateUsr.Password = this.Encrypt(newPassword, salt);
            updateUsr.ResponseQuestion = this.Encrypt(responseQuestion, salt);

            this.Entry<User>(updateUsr).State = EntityState.Modified;
            this._IsSavedOrModified = true;
        }
        public void SendAnnounced(Announced ann)
        {
            this.Announceds.Add(ann);
            this._IsSavedOrModified = true;
        }
        public override int SaveChanges()
        {
            _saltDB.SaveChanges();
            if (_IsSavedOrModified)
                return base.SaveChanges();
            else
                return 0;
        }
        public new void Dispose()
        {
            _saltDB.Dispose();
            base.Dispose();
        }

        private string Encrypt(string val, string salt)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] passBytes = Encoding.Default.GetBytes(val);
            passBytes = sha1.ComputeHash(passBytes);//Хэш пароля
            val = Encoding.Default.GetString(passBytes);

            string passWithSalt = val + salt;
            byte[] passWithSaltBytes = Encoding.Default.GetBytes(passWithSalt);
            passWithSaltBytes = sha1.ComputeHash(passWithSaltBytes);//Хэш пароля с солью
            passWithSalt = Encoding.Default.GetString(passWithSaltBytes);

            return passWithSalt;

        }

        public void SetReadAnnounced(Announced ann)
        {
            if (!ann.IsRead)
            {
                ann.IsRead = true;
                this.Entry<Announced>(ann).State = EntityState.Modified;
                this._IsSavedOrModified = true;
            }
        }

        public void RemoveUser(Guid group_id, User removeUser)
        {
            Group grp = this.GetGroup(group_id);
            if (grp.Users.Contains(removeUser))
            {
                grp.Users.Remove(removeUser);
                this.Entry<Group>(grp).State = EntityState.Modified;
                _IsSavedOrModified = true;
            }
            else if (grp.Administrations.Contains(removeUser))
            {
                grp.Administrations.Remove(removeUser);
                this.Entry<Group>(grp).State = EntityState.Modified;
                _IsSavedOrModified = true;
            }
        }
    }
}
