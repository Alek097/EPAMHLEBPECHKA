using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.Security;


using ChudoPechkaLib.Models;
using ChudoPechkaLib.Data;

namespace ChudoPechkaLib
{
    public class DBManager : IDBManager
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

            if (_db.IsContainUser(login))
            {
                _db.UpdatePassword(login, newPass, responseQuestion);
                return true;
            }
            return false;//TODO:НЕ РАБОТАЕТ СМЕНА ПАРОЛЯ
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
        public bool GetDish(Guid id, out Dish dish)
        {
            dish = null;
            if (_db.IsContainDish(id))
            {
                dish = _db.GetDish(id);
                return true;
            }
            else return false;
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
            else
                throw new HttpException(404, "Группа или приглашение в группу не найдены");
        }
        public void AddAdministrationInGroup(Guid group_id, string login)
        {
            if (_db.IsContainGroup(group_id) && _db.IsContainUser(login))
                _db.AddAuthorInGroup(group_id, login);
            else
                throw new HttpException(404, "Группа или пользователь не найдены");
        }

        public void SetReadAnnounced(Announced ann)
        {
            _db.SetReadAnnounced(ann);
        }

        public void RemoveUser(Guid group_id)
        {
            if (_db.IsContainGroup(group_id))
                _db.RemoveUser(group_id, this.User);
            else
                throw new HttpException(404, "Группа не найдена");
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
            else
                throw new HttpException(404, "Заказ не найден");
        }

        public void RemoveOrder(Guid group_id, Guid order_id)
        {
            if (!_db.IsContainGroup(group_id))
                throw new HttpException(404, "Группа не найдена");
            else if (!_db.IsContainOrder(order_id))
                throw new HttpException(404, "Заказ не найден");
            else
                _db.RemoveOrder(group_id, order_id);
        }
        public void RecoveryOrder(Guid group_id, Guid order_id)
        {
            if (!_db.IsContainGroup(group_id))
                throw new HttpException(404, "Группа не найдена");
            else if (!_db.IsContainOrder(order_id))
                throw new HttpException(404, "Заказ не найден");
            else
                _db.RecoveryOrder(group_id, order_id);
        }

        public void RemoveCancelledOrders(Guid group_id)
        {
            if (_db.IsContainGroup(group_id))
                _db.RemoveCancelledOrders(group_id);
            else
                throw new HttpException(404, "Группа не найдена");
        }

        public void ToOrder(Guid group_id)
        {
            if (_db.IsContainGroup(group_id))
                _db.ToOrder(group_id);
            else
                throw new HttpException(404, "Группа не найдена");
        }

        public void UpdateAvatar(User usr, string fileName)
        {
            _db.UpdateAvatar(usr, fileName);
        }

        public void AddComment(User user, string text, Guid dish_id)
        {
            _db.AddComment(user, text, dish_id);
        }

        public void RemoveComment(Guid comment_id)
        {
            if (_db.IsContainComment(comment_id))
            {
                Comment comment = _db.GetComment(comment_id);

                if (!this.User.Equals(comment.User))
                    throw new HttpException(423, "Доступ запрещён");

                _db.RemoveComment(comment_id);
            }
            else
                throw new HttpException(404, "Комментарий не найден");
        }

        public bool GetComment(Guid id, out Comment comment)
        {
            comment = null;
            if (_db.IsContainComment(id))
            {
                comment = _db.GetComment(id);
                return true;
            }
            else
                return false;
        }

        public void UpdateComment(Guid comment_id, string newText)
        {
            Comment comment = null;
            if (this.GetComment(comment_id, out comment))
            {
                if (comment.User.Equals(this.User))
                {
                    _db.UpdateComment(comment, newText);
                }
                else
                    throw new HttpException(423, "Доступ запрещён");
            }
            else
                throw new HttpException(404, "Комментарий не найден");
        }

        public void AddMoney(string login, uint addMoney)
        {
            User usr = null;

            if (!this.User.Login.Equals(login))
                throw new HttpException(423, "Доступ запрещён");
            else if (!this.GetUser(login, out usr))
                throw new HttpException(404, "Пользователь не найден");
            else
                _db.AddMoney(usr, addMoney);
        }

        public void TransferMoney(string from, string to, uint money)
        {
            User usrFrom = null;
            User usrTo = null;

            if (!this.User.Login.Equals(from))
                throw new HttpException(423, "Доступ запрещён");
            else if (!this.GetUser(from, out usrFrom) || !this.GetUser(to, out usrTo))
                throw new HttpException(404, "Пользователь не найден");
            else
                _db.TransferMoney(usrFrom, usrTo, money);
        }
    }
}
