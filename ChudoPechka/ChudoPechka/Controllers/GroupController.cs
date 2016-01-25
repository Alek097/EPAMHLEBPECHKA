using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChudoPechka.Controllers
{
    public class GroupController : ChudoPechka.Controllers.Base.BaseController
    {
        public ActionResult Index(Guid Group_id)
        {
            return View();
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
            if(Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            if (string.IsNullOrEmpty(gName))
            {
                ModelState.AddModelError("", "Нулевая строка");
                return View();
            }

            Auth.RegisterGroup(gName);

            return Redirect(Url.Action("Index"));
        }
    }
}