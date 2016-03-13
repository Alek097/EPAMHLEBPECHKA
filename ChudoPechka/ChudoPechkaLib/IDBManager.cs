using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using ChudoPechkaLib.Models;
using ChudoPechkaLib.Data;

namespace ChudoPechkaLib
{
    public interface IDBManager
    {
        User User { get; set; }
        bool IsAuthentication { get; set; }
        void Start(HttpContext context, IStoreDB db);
        bool LoginIn(string login, string password);
        void LoginOut();
        void RegisterUser(User newUser);
        Guid RegisterGroup(string Name);
        void UpdateOrder(Order order);
        bool UpdatePassword(string login, string newPass);
        bool GetUser(string login, out User usr);
        bool GetUsersForEmail(string e_Mail, out List<User> users);
        bool GetGroup(Guid id, out Group grp);
        bool GetOrder(Guid id, out Order ord);
        bool GetDish(Guid id, out Dish dish);
        bool GetComment(Guid id, out Comment comment);
        void SendAnnounced(Announced ann);
        void AddMoney(string login, uint addMoney);
        void AddComment(User user, string text, Guid dish_id);
        void AddMemberInGroup(Guid Group_id);
        void AddAdministrationInGroup(Guid Group_Id, string login);
        void RemoveUser(Guid group_id);
        void RemoveOrder(Guid order_id);
        void RemoveOrder(Guid group_id, Guid order_id);
        void RemoveCancelledOrders(Guid group_id);
        void RemoveComment(Guid comment_id);
        void RecoveryOrder(Guid group_id, Guid order_id);
        void SetReadAnnounced(Announced ann);
        void SetActiveCode(User usr);
        void ToOrder(Guid group_id);
        void ToOrder(Order order);
        Task SendConfirmCodeAsync(string login, string e_Mail);
        void TransferMoney(string from, string to, uint money);
        void UpdateAvatar(User usr, string fileName);
        void UpdateComment(Guid comment_id, string newText);
    }
}
