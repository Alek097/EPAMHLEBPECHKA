using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public interface IStoreDB : IDisposable
    {
        User GetUser(string login);
        Group GetGroup(Guid group_id);
        Order GetOrder(Guid order_id);
        Dish GetDish(Guid dish_id);
        Comment GetComment(Guid comment_id);
        List<User> GetUsersForEmail(string e_Mail);
        void AddUser(User usr);
        void AddMemberInGroup(Guid group_id, User usr);
        void AddAuthorInGroup(Guid group_id, string login);
        void AddGroup(Group grp);
        void AddMoney(User usr, uint addMoney);
        void AddComment(User user, string text, Guid dish_id);
        void AddOrder(Order order);
        Guid AddDish(string nameDish);
        bool IsContainUserEmail(string e_Mail);
        bool IsContainGroup(Guid group_id);
        bool IsContainDish(Guid dish_id);
        bool IsContainComment(Guid comment_id);
        bool IsContainUser(string login, string pass);
        bool IsContainUser(string login);
        bool IsContainUser(Guid usr_id);
        bool IsContainAnnounced(Guid From_id);
        bool IsContainOrder(Guid order_id);
        void UpdateOrder(Order order);
        void UpdatePassword(string login, string newPassword);
        void UpdateAvatar(User usr, string fileName);
        void UpdateComment(Comment comment, string newText);
        void SendAnnounced(Announced ann);
        void SetConfirmCode(User usr);
        void SetReadAnnounced(Announced ann);
        void SetActiveCode(User usr);
        void RemoveUser(Guid group_id, User removeUser);
        void RemoveOrder(Guid order_id);
        void RemoveCancelledOrders(Guid group_id);
        void RemoveComment(Guid comment_id);
        void RemoveOrder(Guid group_id, Guid order_id);
        void RecoveryOrder(Guid group_id, Guid order_id);
        void ToOrder(Guid group_id);
        void TransferMoney(User from, User to, uint money);
        int SaveChanges();
    }
}
