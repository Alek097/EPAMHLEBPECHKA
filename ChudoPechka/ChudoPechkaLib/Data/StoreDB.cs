using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Data.Entity;

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
                .HasOptional<Author>(a => a.Author)
                .WithRequired(a => a.User);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Announced> Announceds { get; set; }

        public User GetUser(string login)
        {
            return this.Users
                .Include(u => u.Author)
                .Include(u => u.Author.Groups)
                .Include(u => u.Groups)
                .Include(u => u.Announceds)
                .FirstOrDefault((usr) => usr.Login.Equals(login));
        }
        public Group GetGroup(Guid group_id)
        {
            return this.Groups
                .Include(g => g.Author)
                .Include(g => g.Author.User)
                .Include(g => g.Users)
                .First(g => g.Id.Equals(group_id));
        }
        public void AddUser(User usr)
        {
            usr.Password = this.EncryptPass(usr.Password, _saltDB.GetSalts(usr.Id));

            this.Users.Add(usr);
            this.Authors.Add((Author)usr);
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
            updateGrp.Users.Add(usr);
            this.Entry<Group>(updateGrp).State = EntityState.Modified;

            this._IsSavedOrModified = true;
        }
        public bool IsContainGroup(Guid group_id)
        {
            try
            {
                this.Groups.First((u) => u.Id.Equals(group_id));
                return true;
            }
            catch(InvalidOperationException)
            {
                return false;
            }
        }
        public bool IsContainUser(string login, string pass)
        {
            User usr = this.GetUser(login);
            string encryptPass = this.EncryptPass(pass, _saltDB.GetSalts(usr.Id));
            try
            {
                this.Users.First((u) => u.Login.Equals(login) && u.Password.Equals(encryptPass));
                return true;
            }
            catch(InvalidOperationException)
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
            catch(InvalidOperationException)
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
            catch(InvalidOperationException)
            {
                return false;
            }
        }
        public bool ResponceOnQuestion(string login, string response)
        {
            try
            {
                this.Users.First(u => u.Login.Equals(login) && u.ResponseQuestion.Equals(response));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void UpdatePassword(string login, string newPassword)
        {
            User updateUsr = this.Users.First(u => u.Login.Equals(login));
            updateUsr.Password = this._saltDB.UpdateSalt(updateUsr.Id);
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

        private string EncryptPass(string pass,string salt)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] passBytes = Encoding.Default.GetBytes(pass);
            passBytes = sha1.ComputeHash(passBytes);//Хэш пароля
            pass = Encoding.Default.GetString(passBytes);

            string passWithSalt = pass + salt;
            byte[] passWithSaltBytes = Encoding.Default.GetBytes(passWithSalt);
            passWithSaltBytes = sha1.ComputeHash(passWithSaltBytes);//Хэш пароля с солью
            passWithSalt = Encoding.Default.GetString(passWithSaltBytes);

            return passWithSalt;

        }
    }
}
