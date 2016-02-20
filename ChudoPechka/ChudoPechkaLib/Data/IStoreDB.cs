using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public interface IStoreDB :IDisposable
    {
        User GetUser(string login);
        Group GetGroup(Guid group_id);
        void AddUser(User usr);
        void AddMemberInGroup(Guid group_id, User usr);
        void AddGroup(Group grp);
        bool IsContainGroup(Guid group_id);
        bool IsContainUser(string login, string pass);
        bool IsContainUser(string login);
        bool IsContainAnnounced(Guid From_id);
        bool ResponceOnQuestion(string login, string response);
        void UpdatePassword(string login, string newPassword);
        void SendAnnounced(Announced ann);
        void SetReadAnnounced(Announced ann);
        int SaveChanges();
    }
}
