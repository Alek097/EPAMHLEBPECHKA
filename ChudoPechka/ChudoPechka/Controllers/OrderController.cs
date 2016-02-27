using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;
using ChudoPechkaLib;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class OrderController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ToOrder()
        {
            if (Auth.IsAuthentication)
            {
                OrderModel model = new OrderModel();
                return View(model);
            }
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToOrder(OrderModel model)
        {
            if(!Auth.IsAuthentication && !ModelState.IsValid)
                return Redirect(Url.Action("Index", "Home"));
            else
                return View(model);
        }
    }
}