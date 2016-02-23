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
        }
        [ValidateAntiForgeryToken]
        public void AddAuthor(Guid Group_Id, string login)
        {
            if(Auth.IsAuthentication)
            {
                Auth.AddAuthorInGroup(Group_Id, login);
            }
        }
        [ValidateAntiForgeryToken]
        public void RemoveUser(Guid group_id)
        {
            if(Auth.IsAuthentication)
            {
                Auth.RemoveUser(group_id);
            }
        }
    }
}