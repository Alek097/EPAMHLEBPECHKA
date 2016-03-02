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
            if (Auth.IsAuthentication)
                return View(Auth.User.Orders.ToList());
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        public ActionResult Edit(Guid Order_id)
        {
            if (Auth.IsAuthentication)
            {
                Order ord;
                if (Auth.GetOrder(Order_id, out ord))
                {
                    if (Auth.User.Orders.Contains(ord))
                    {
                        EditModel model = ord;
                        return View(model);
                    }
                    else
                        throw new HttpException(423, "Доступ запрещён");
                }
                else
                    throw new HttpException(404, "Заказ не найден");

            }
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditModel model)
        {
            if (Auth.IsAuthentication || !model.Status.Equals("Заказан") || !model.IsOrdered)
            {
                if (!ModelState.IsValid)
                    return View(model);
                else
                {
                    Auth.UpdateOrder(model);
                    return Redirect("Index");
                }
            }
            else
                return Redirect(Url.Action("Index", "Home"));
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
            if (!Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else if (ModelState.IsValid)
            {

                Auth.ToOrder(model);

                model = new OrderModel();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(Guid order_id)
        {
            Auth.RemoveOrder(order_id);
            return Redirect("index");
        }
    }
}