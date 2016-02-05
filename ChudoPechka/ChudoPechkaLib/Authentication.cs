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
        private void SetDate(string login)
        {

        }

        public void Start(HttpContext context)
        {

        }
        public bool LoginIn(string login, string password)
        {
                return false;
        }
        public void LoginOut()
        {
            this.DeleteCookies();
        }
        public void RegisterUser(string login, string password, string firstName, string secondName, string secretQuestion, string responseQuestion, DateTime birthDay)
        {

            this.LoginIn(login, password);
        }
        public void RegisterGroup(string Name)
        {
            
        }

        public bool UpdatePassword(string login, string newPass, string responseQuestion)
        {
            return true;
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
