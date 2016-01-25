using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Security;

using ChudoPechkaLib.Data.Model;
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

        private User _user;
        private HttpContext _httpContext;
        private void SetDate(string login)
        {
            using (UsersStoreDB db = new UsersStoreDB())
            {
                try
                {
                    this._user = db.Get_User(login);
                    this.IsAuthentication = true;
                }
                catch (ArgumentException)
                {
                    this._user = null;
                    this.IsAuthentication = false;
                }
            }
        }

        public void Start(HttpContext context)
        {
            this._httpContext = context;
            HttpCookie cookie = context.Request.Cookies[COOKIE_NAME];

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                using (UsersStoreDB db = new UsersStoreDB())
                    if (db.HasUser(ticket.Name))
                        this.SetDate(ticket.Name);
            }
        }
        public bool LoginIn(string login, string password)
        {
            using(UsersStoreDB db = new UsersStoreDB())
            if (db.HasUser(login, password))
            {
                this.CreateCookie(login);
                return true;
            }
            else
                return false;
        }
        public void LoginOut()
        {
            this.DeleteCookies();
        }
        public void RegisterUser(string login, string password, string firstName, string secondName, string secretQuestion, string responseQuestion, DateTime birthDay)
        {
            using (UsersStoreDB db = new UsersStoreDB())
                db.Add_User(
                    login,
                    password,
                    firstName,
                    secondName,
                    secretQuestion,
                    responseQuestion,
                    birthDay);

            this.LoginIn(login, password);
        }
        public void RegisterGroup(string Name)
        {
            using (GroupsStoreDB db = new GroupsStoreDB())
                db.Add_Group(Name, this._user.Id);
            
        }

        public bool UpdatePassword(string login, string newPass, string responseQuestion)
        {
            using (UsersStoreDB db = new UsersStoreDB())
            {
                return db.UpdatePassword(login, newPass, responseQuestion);
            }
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
