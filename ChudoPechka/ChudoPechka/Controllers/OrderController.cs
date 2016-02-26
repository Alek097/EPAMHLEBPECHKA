using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChudoPechka.Controllers
{
    public class OrderController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ToOrder()
        {
            if (Auth.IsAuthentication)
            {
                ViewBag.Day = day;
                return View();
            }
            else
                return Redirect(Url.Action("LoginIn", "Account"));
        }
    }
}