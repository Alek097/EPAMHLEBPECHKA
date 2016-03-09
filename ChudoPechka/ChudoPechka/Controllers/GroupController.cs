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
            if (Auth.GetGroup(Group_id, out grp))
                return View(grp);
            throw new HttpException(404, "Группа не найдена");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Auth.IsAuthentication)
                return View();
            return Redirect(Url.Action("Index", "Home"));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(string gName)
        {
            if (!Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else if (string.IsNullOrEmpty(gName))
            {
                ModelState.AddModelError("", "Нулевая строка");
                return View();
            }
            else
            {
                Guid grp_id = Auth.RegisterGroup(gName);

                return Redirect(Url.Action("Index", new { Group_id = grp_id }));
            }
        }
        public ActionResult My()
        {
            if (Auth.IsAuthentication)
                return View(Auth.User);
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [ValidateAntiForgeryToken]
        public void AddUser(Guid Group_Id)
        {
            if (Auth.IsAuthentication)
            {
                Auth.AddMemberInGroup(Group_Id);
            }
            else
                throw new HttpException(401, "Вы не авторизированы");
        }
        [ValidateAntiForgeryToken]
        public void AddAuthor(Guid Group_Id, string login)
        {
            if (Auth.IsAuthentication)
            {
                Auth.AddAuthorInGroup(Group_Id, login);
            }
            else
                throw new HttpException(401, "Вы не авторизированы");
        }
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(Guid group_id)
        {
            if (Auth.IsAuthentication)
            {
                Auth.RemoveUser(group_id);
                return Redirect(Url.Action("My"));
            }
            else
                throw new HttpException(401, "Вы не авторизированы");
        }
        public ActionResult OrderInf(Guid Group_Id)
        {
            Group grp = null;
            if (!Auth.IsAuthentication)
                throw new HttpException(401, "Вы не авторизированы");
            else if (!Auth.GetGroup(Group_Id, out grp))
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
            if (!Auth.IsAuthentication)
                throw new HttpException(401, "Вы не авторизированы");
            try
            {
                Auth.RemoveOrder(Group_id, Order_id);
                throw new HttpException(200, "Закзаз удалён из списка группы");
            }
            catch (InvalidOperationException ex)
            {
                throw new HttpException(404, ex.Message);
            }

        }
        [ValidateAntiForgeryToken]
        public ActionResult RecoveryOrder(Guid Group_id, Guid Order_id)
        {
            if (!Auth.IsAuthentication)
                throw new HttpException(401, "Вы не авторизированы");
            try
            {
                Auth.RecoveryOrder(Group_id, Order_id);
                throw new HttpException(200, "Закзаз удалён из списка группы");
            }
            catch (InvalidOperationException ex)
            {
                throw new HttpException(404, ex.Message);
            }
        }
        [ValidateAntiForgeryToken]
        public ActionResult ToOrder(Guid Group_id)
        {
            if (!Auth.IsAuthentication)
                throw new HttpException(401, "Вы не авторизированы");
            Auth.ToOrder(Group_id);
            return Redirect(Url.Action("OrderInf", new { Group_id = Group_id }));
        }
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCancelledOrders(Guid Group_id)
        {
            if (!Auth.IsAuthentication)
                throw new HttpException(401, "Вы не авторизированы");
            Auth.RemoveCancelledOrders(Group_id);
            return Redirect(Url.Action("OrderInf", new { Group_id = Group_id }));
        }
    }
}