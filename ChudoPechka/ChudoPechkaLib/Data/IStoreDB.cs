﻿using System;
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
        Order GetOrder(Guid order_id);
        void AddUser(User usr);
        void AddMemberInGroup(Guid group_id, User usr);
        void AddAuthorInGroup(Guid group_id, string login);
        void AddGroup(Group grp);
        void AddOrder(Order order);
        bool IsContainGroup(Guid group_id);
        bool IsContainUser(string login, string pass);
        bool IsContainUser(string login);
        bool IsContainUser(Guid usr_id);
        bool IsContainAnnounced(Guid From_id);
        bool IsContainOrder(Guid order_id);
        bool ResponceOnQuestion(string login, string response);
        void UpdateOrder(Order order);
        void UpdatePassword(string login, string newPassword,string responseQuestion);
        void SendAnnounced(Announced ann);
        void SetReadAnnounced(Announced ann);
        void RemoveUser(Guid group_id, User removeUser);
        void RemoveOrder(Guid order_id);
        int SaveChanges();
    }
}
