using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;

namespace ChudoPechkaLib.Data.Model
{
    public class User : IUser
    {
        #region Данные
        public DateTime BirthDay { get; set; }

        public string FirsName { get; set; }

        public string Login { get; set; }

        public string ResponseQuestion { get; set; }

        public string SecondName { get; set; }

        public string SecretQuestion { get; set; }

        public string Password { get; set; }
        #endregion
        private const string COOKIE_NAME = "_TEST_COOKIE";

        public HttpContext HttpContext { get; private set; }

        public bool IsIAuthentication { get; set; }
        private void SetDate(CarrierUser carrier)
        {
            this.Login = carrier.Login;
            this.Password = carrier.Password;
            this.FirsName = carrier.FirsName;
            this.SecondName = carrier.SecondName;
            this.BirthDay = carrier.BirthDay;
            this.SecretQuestion = carrier.SecondName;
            this.ResponseQuestion = carrier.ResponseQuestion;
        }

        public void Start(HttpContext context)
        {
            this.HttpContext = context;
            HttpCookie cookie = context.Request.Cookies[COOKIE_NAME];

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if(UsersStoreDB.HasUser(ticket.Name))
                        
            }
        }
    }
}
