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
        [HttpPost]
        public ActionResult Create(string gName)
        {
            if(!Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            if (string.IsNullOrEmpty(gName))
            {
                ModelState.AddModelError("", "Нулевая строка");
                return View();
            }

            Guid grp_id = Auth.RegisterGroup(gName);

            return Redirect(Url.Action("Index",new {Group_id = grp_id }));
        }
    }
}