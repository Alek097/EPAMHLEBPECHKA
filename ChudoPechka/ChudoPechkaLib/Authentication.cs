using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Web.Security;

using ChudoPechkaLib.Models;
using ChudoPechkaLib.Data;

namespace ChudoPechkaLib
{
    public class Authentication : IAuthentication
    {
        private const string COOKIE_NAME = "_TEST_COOKIE";//TODO: По завершению дать нормальное название
        private IStoreDB _db;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                throw new ReadOnlyException();
            }
        }
        public bool IsAuthentication { get; set; }
        private User _user;
        private HttpContext _httpContext;

        public void Start(HttpContext context, IStoreDB db)
        {
            this._db = db;
            this._httpContext = context;
            HttpCookie cookie = context.Request.Cookies[COOKIE_NAME];

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                this._user = db.GetUser(ticket.Name);

                if (this.User != null)
                    this.IsAuthentication = true;
                else
                {
                    this.DeleteCookies();
                    this.IsAuthentication = false;
                }
            }
        }
        public bool LoginIn(string login, string password)
        {
            if (_db.IsContainUser(login, password))
            {
                this.CreateCookie(login);
                return true;
            }
            return false;
        }
        public void LoginOut()
        {
            this.DeleteCookies();
        }
        public void RegisterUser(User newUser)
        {
            _db.AddUser(newUser);
        }
        public Guid RegisterGroup(string name)
        {
            Group newGroup = new Group(name, this.User);
            _db.AddGroup(newGroup);

            return newGroup.Id;
        }

        public bool UpdatePassword(string login, string newPass, string responseQuestion)
        {

            if (_db.IsContainUser(login) && _db.ResponceOnQuestion(login, responseQuestion))
            {
                _db.UpdatePassword(login, newPass, responseQuestion);
                return true;
            }
            return false;
        }
        private void DeleteCookies()
        {
            this._httpContext.Response.Cookies[COOKIE_NAME].Value = string.Empty;
        }
        private void CreateCookie(string login)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                login,
                true,
                5000);

            string encryTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(COOKIE_NAME);
            cookie.Value = encryTicket;

            this._httpContext.Response.Cookies.Set(cookie);
        }
        public bool GetUser(string login, out User usr)
        {
            usr = null;

            if (_db.IsContainUser(login))
            {
                usr = _db.GetUser(login);
                return true;
            }
            return false;
        }

        public bool GetGroup(Guid id, out Group grp)
        {
            grp = null;

            if (_db.IsContainGroup(id))
            {
                grp = _db.GetGroup(id);
                return true;
            }
            return false;
        }

        public void SendAnnounced(Announced ann)
        {
            _db.SendAnnounced(ann);
        }

        public void AddMemberInGroup(Guid group_id)
        {
            User usr = this.User;
            if (_db.IsContainGroup(group_id) && _db.IsContainAnnounced(group_id))
                _db.AddMemberInGroup(group_id, usr);
        }
        public void AddAuthorInGroup(Guid group_id, string login)
        {
            if (_db.IsContainGroup(group_id) && _db.IsContainUser(login))
                _db.AddAuthorInGroup(group_id, login);
        }

        public void SetReadAnnounced(Announced ann)
        {
            _db.SetReadAnnounced(ann);
        }

        public void RemoveUser(Guid group_id)
        {
            _db.RemoveUser(group_id, this.User);
        }

        public void ToOrder(Order order)
        {
            _db.AddOrder(order);
        }

        public bool GetOrder(Guid id, out Order ord)
        {
            ord = null;
            if (_db.IsContainOrder(id))
            {
                ord = _db.GetOrder(id);
                return true;
            }
            else
                return false;
        }

        public void UpdateOrder(Order order)
        {
            if (_db.IsContainOrder(order.Id))
                _db.UpdateOrder(order);
        }

        public void RemoveOrder(Guid order_id)
        {
            if (_db.IsContainOrder(order_id))
                _db.RemoveOrder(order_id);
        }

        public void RemoveOrder(Guid group_id, Guid order_id)
        {
            if (!_db.IsContainGroup(group_id))
                throw new InvalidConstraintException("Группа не найдена");
            else if (!_db.IsContainOrder(order_id))
                throw new InvalidConstraintException("Заказ не найден");
            else
                _db.RemoveOrder(group_id, order_id);
        }
        public void RecoveryOrder(Guid group_id, Guid order_id)
        {
            if (!_db.IsContainGroup(group_id))
                throw new InvalidOperationException("Группа не найдена");
            else if (!_db.IsContainOrder(order_id))
                throw new InvalidOperationException("Заказ не найден");
        }
    }
}
