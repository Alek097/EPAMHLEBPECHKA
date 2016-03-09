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
                .Map(m => m.ToTable("Administration"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Groups)
                .WithMany(g => g.Users)
                .Map(m => m.ToTable("UsersGroups"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithRequired(c => c.User);

            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Comments)
                .WithRequired(c => c.Dish);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Announced> Announceds { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public Order GetOrder(Guid order_id)
        {
            return this.Orders
                .Include(o => o.Groups)
                .Include(o => o.User)
                .First(o => o.Id.Equals(order_id));
        }
        public User GetUser(string login)
        {
            return this.Users
                .Include(u => u.Groups)
                .Include(u => u.Announceds)
                .Include(u => u.Orders)
                .FirstOrDefault((usr) => usr.Login.Equals(login));
        }
        public User GetUser(Guid usr_id)
        {
            return this.Users
                .Include(u => u.Groups)
                .Include(u => u.Announceds)
                .Include(u => u.Orders)
                .First((usr) => usr.Id.Equals(usr_id));
        }
        public Group GetGroup(Guid group_id)
        {
            return this.Groups
               .Include(g => g.Administrations)
               .Include(g => g.Users)
               .Include(g => g.Orders)
               .First(g => g.Id.Equals(group_id));
        }
        public Dish GetDish(Guid dish_id)
        {
            return this.Dishes
                .Include(d => d.Comments)
                .First(d => d.Id.Equals(dish_id));
        }
        public Comment GetComment(Guid comment_id)
        {
            return this.Comments
                .Include(c => c.User)
                .Include(c => c.Dish)
                .First(c => c.Id.Equals(comment_id));
        }
        public Guid AddDish(string nameDish)
        {
            if (!this.IsContainDish(nameDish))
            {
                Dish dish = new Dish();
                dish.Name = nameDish;
                this.Dishes.Add(dish);
                base.SaveChanges();//Т.к. в отличие от остальных методов он вызывается из статического экземпляра,а не из ninject

                return dish.Id;
            }
            else
            {
                return this.Dishes.First(d => d.Name.Equals(nameDish)).Id;
            }
        }
        public void AddUser(User usr)
        {
            string salt = _saltDB.GetSalt(usr.Id);

            usr.Password = this.Encrypt(usr.Password, salt);
            usr.ResponseQuestion = this.Encrypt(usr.ResponseQuestion, salt);
            usr.AvatarPath = "~/img/Standart/Avatar.jpg";

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
                this.Groups.First((u) => u.Id == group_id);
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
        public bool IsContainComment(Guid comment_id)
        {
            try
            {
                this.Comments.First(c => c.Id.Equals(comment_id));
                return true;
            }
            catch(InvalidCastException)
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
                this.Users.First((u) => u.Id == usr_id);
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
                this.Announceds.First(a => a.From == From_id);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool IsContainDish(Guid dish_id)
        {
            try
            {
                this.Dishes.First(d => d.Id.Equals(dish_id));
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
        public bool IsContainOrder(Guid order_id)
        {
            try
            {
                this.Orders.First((o) => o.Id == order_id);
                return true;
            }
            catch (InvalidOperationException)
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

        public void AddOrder(Order order)
        {
            this.Orders.Add(order);
            _IsSavedOrModified = true;
        }

        public void UpdateOrder(Order order)
        {
            Order oldOrd = this.GetOrder(order.Id);
            oldOrd.Groups = order.Groups;
            oldOrd.Day = order.Day;
            oldOrd.IsOrdered = order.IsOrdered;
            oldOrd.Price = order.Price;
            oldOrd.Status = order.Status;
            oldOrd.Type = order.Type;
            this.Entry<Order>(oldOrd).State = EntityState.Modified;
            _IsSavedOrModified = true;
        }

        public void RemoveOrder(Guid order_id)
        {
            Order remOrder = this.GetOrder(order_id);
            if (!remOrder.IsOrdered)
                this.Entry<Order>(remOrder).State = EntityState.Deleted;
            else
            {
                remOrder.Status = "Отменён";
                this.Entry<Order>(remOrder).State = EntityState.Modified;
            }
            _IsSavedOrModified = true;
        }

        public void RemoveOrder(Guid group_id, Guid order_id)
        {
            Group grp = this.GetGroup(group_id);
            Order ord = this.GetOrder(order_id);

            grp.Orders.Remove(ord);

            this.Entry<Group>(grp).State = EntityState.Modified;
            _IsSavedOrModified = true;
        }

        public void RecoveryOrder(Guid group_id, Guid order_id)
        {
            Group grp = this.GetGroup(group_id);
            Order ord = this.GetOrder(order_id);

            grp.Orders.Add(ord);

            this.Entry<Group>(grp).State = EntityState.Modified;
            _IsSavedOrModified = true;
        }

        public void ToOrder(Guid group_id)
        {
            Group grp = this.GetGroup(group_id);

            foreach (Order order in grp.Orders)
                if (!order.Status.Equals("Отменён"))
                {
                    order.IsOrdered = true;
                    order.Status = "Заказан";
                }
            this.Entry<Group>(grp).State = EntityState.Modified;
            _IsSavedOrModified = true;
        }

        public void RemoveCancelledOrders(Guid group_id)
        {
            Group grp = this.GetGroup(group_id);

            Order[] orders = grp.Orders.Where(o => o.Status.Equals("Отменён")).ToArray();

            for (int i = 0; i < orders.Length; i++)
                this.Entry<Order>(orders[i]).State = EntityState.Deleted;

            this.Entry<Group>(grp).State = EntityState.Modified;

            _IsSavedOrModified = true;
        }

        public void UpdateAvatar(string login, string fileName)
        {
            User usr = this.GetUser(login);

            usr.AvatarPath = fileName;

            this.Entry<User>(usr).State = EntityState.Modified;

            _IsSavedOrModified = true;
        }
        private bool IsContainDish(string nameDish)
        {
            try
            {
                this.Dishes.First(d => d.Name.Equals(nameDish));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public void AddComment(string login, string text, Guid dish_id)
        {
            Dish dish = this.GetDish(dish_id);
            User user = this.GetUser(login);

            Comment comment = new Comment();
            comment.User = user;
            comment.Dish = dish;
            comment.Text = text;
            comment.Date = DateTime.Now;

            dish.Comments.Add(comment);

            this.Comments.Add(comment);

            _IsSavedOrModified = true;
        }

        public void RemoveComment(Guid comment_id)
        {
            Comment comment = this.GetComment(comment_id);

            this.Entry<Comment>(comment).State = EntityState.Deleted;

            _IsSavedOrModified = true;
        }

        public void UpdateComment(Guid comment_id, string newText)
        {
            Comment modifComment = this.GetComment(comment_id);

            modifComment.Text = newText;

            this.Entry<Comment>(modifComment).State = EntityState.Modified;

            _IsSavedOrModified = true;
        }
    }
}
