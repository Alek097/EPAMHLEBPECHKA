using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using ChudoPechkaLib.Data.Model;

namespace ChudoPechkaLib
{
    public interface IAuthentication
    {
        User User { get; set; }
        bool IsAuthentication { get; set; }
        void Start(HttpContext context);
        bool LoginIn(string login, string password);
        void LoginOut();
        void RegisterUser(string login, string password, string firstName, string secondName, string secretQuestion, string responseQuestion, DateTime birthDay);
        void RegisterGroup(string Name);
        bool UpdatePassword(string login, string newPass, string responseQuestion);
    }
}
