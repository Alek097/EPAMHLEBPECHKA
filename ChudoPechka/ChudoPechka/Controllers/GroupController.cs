using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechkaLib.Models;

using ChudoPechka.Models;

namespace ChudoPechka.Controllers
{
    public class GroupController : ChudoPechka.Controllers.Base.BaseController
    {
        public ActionResult Index(Guid Group_id)
        {
            Group grp;
            if (Manager.GetGroup(Group_id, out grp))
                return View(grp);
            else
                throw new HttpException(404, "Группа не найдена");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Manager.IsAuthentication)
                return View();
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(string gName)
        {
            if (!Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else if (string.IsNullOrEmpty(gName))
            {
                ModelState.AddModelError("", "Нулевая строка");
                return View();
            }
            else
            {
                Guid grp_id = Manager.RegisterGroup(gName);

                return Redirect(Url.Action("Index", new { Group_id = grp_id }));
            }
        }
        public ActionResult My()
        {
            if (Manager.IsAuthentication)
                return View(Manager.User);
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [ValidateAntiForgeryToken]
        public void AddUser(Guid Group_Id)
        {
            if (Manager.IsAuthentication)
            {
                Manager.AddMemberInGroup(Group_Id);
            }
            else
                throw new HttpException(401, "Вы не авторизованы");
        }
        [ValidateAntiForgeryToken]
        public void AddAuthor(Guid Group_Id, string login)
        {
            if (Manager.IsAuthentication)
            {
                Manager.AddAdministrationInGroup(Group_Id, login);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(401, "Вы не авторизованы");
        }
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(Guid group_id)
        {
            if (Manager.IsAuthentication)
            {
                Manager.RemoveUser(group_id);
                return Redirect(Url.Action("My"));
            }
            else
                throw new HttpException(401, "Вы не авторизованы");
        }
        public ActionResult OrderInf(Guid Group_Id)
        {
            Group grp = null;
            if (!Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else if (!Manager.GetGroup(Group_Id, out grp))
                throw new HttpException(404, "Группа не найдена");
            else
            {
                ViewData["Group_id"] = grp.Id;
                return View(grp.Orders.Where(o => o.Day == DateTime.Now.Date.AddDays(1)).ToList());//На завтра
            }
        }
        [ValidateAntiForgeryToken]
        public ActionResult RemoveOrder(Guid Group_id, Guid Order_id)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");

            Manager.RemoveOrder(Group_id, Order_id);
            throw new HttpException(200, "Закзаз удалён из списка группы");

        }
        [ValidateAntiForgeryToken]
        public ActionResult RecoveryOrder(Guid Group_id, Guid Order_id)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");

            Manager.RecoveryOrder(Group_id, Order_id);
            throw new HttpException(200, "Закзаз удалён из списка группы");
        }
        [ValidateAntiForgeryToken]
        public ActionResult ToOrder(Guid Group_id)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");
            Manager.ToOrder(Group_id);
            return Redirect(Url.Action("OrderInf", new { Group_id = Group_id }));
        }
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCancelledOrders(Guid Group_id)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");
            Manager.RemoveCancelledOrders(Group_id);
            return Redirect(Url.Action("OrderInf", new { Group_id = Group_id }));
        }
    }
}