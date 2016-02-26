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
<<<<<<< HEAD
        public ActionResult ToOrder()
=======
        public ActionResult ToOrder(int day)
>>>>>>> b3d1eb50117e9d498b36bb16779cde060ebcc83f
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