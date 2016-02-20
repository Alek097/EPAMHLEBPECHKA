using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using ChudoPechkaLib.Models;
using ChudoPechkaLib.Data;

namespace ChudoPechkaLib
{
    public interface IAuthentication
    {
        User User { get; set; }
        bool IsAuthentication { get; set; }
        void Start(HttpContext context, IStoreDB db);
        bool LoginIn(string login, string password);
        void LoginOut();
        void RegisterUser(User newUser);
        Guid RegisterGroup(string Name);
        bool UpdatePassword(string login, string newPass, string responseQuestion);
        bool GetUser(string login, out User usr);
        bool GetGroup(Guid id, out Group grp);
        void SendAnnounced(Announced ann);
        void AddMemberInGroup(Guid Group_id, User usr);
        void SetReadAnnounced(Announced ann);
    }
}
