using System;
using System.Collections.Generic;
using System.Linq;
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

        User IAuthentication.User
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        private User _user;
        private HttpContext _httpContext;

        public void Start(HttpContext context)
        {
            this._httpContext = context;
            HttpCookie cookie = context.Request.Cookies[COOKIE_NAME];

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                using (StoreDB db = new StoreDB())
                {
                    this._user = db.GetUser(ticket.Name);
                    this.IsAuthentication = this.User != null ? true : false;
                }
            }
        }
        public bool LoginIn(string login, string password)
        {
            using (StoreDB db = new StoreDB())
            {
                if (db.IsContainUser(login, password))
                {
                    this.CreateCookie(login);
                    return true;
                }
            }
            return false;
        }
        public void LoginOut()
        {
            this.DeleteCookies();
        }
        public void RegisterUser(User newUser)
        {
            using (StoreDB db = new StoreDB())
                db.AddUser(newUser);
        }
        public void RegisterGroup(string name)
        {
            using (StoreDB db = new StoreDB())
                db.AddGroup(new Group(name, this.User));
        }

        public bool UpdatePassword(string login, string newPass, string responseQuestion)
        {

            using (StoreDB db = new StoreDB())
            {
                if (db.IsContainUser(login) && db.ResponceOnQuestion(login, responseQuestion))
                {
                    db.UpdatePassword(login, newPass);
                    return true;
                }
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
    }
}
