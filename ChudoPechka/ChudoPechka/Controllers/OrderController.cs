using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;
using ChudoPechka.Filters;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class OrderController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Order
        [AlllActive]
        public ActionResult Index()
        {
            if (Manager.IsAuthentication)
                return View(Manager.User.Orders.Where(o => !o.Status.Equals("Отменён")).ToList());
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [AlllActive]
        public ActionResult Edit(Guid Order_id)
        {
            if (Manager.IsAuthentication)
            {
                Order ord;
                if (Manager.GetOrder(Order_id, out ord))
                {
                    if (Manager.User.Orders.Contains(ord))
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
        [AlllActive]
        public ActionResult Edit(EditModel model)
        {
            if (Manager.IsAuthentication || !model.Status.Equals("Заказан") || !model.IsOrdered)
            {
                if (!ModelState.IsValid)
                    return View(model);
                else
                {
                    Manager.UpdateOrder(model);
                    return Redirect("Index");
                }
            }
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [HttpGet]
        [AlllActive]
        public ActionResult ToOrder()
        {
            if (Manager.IsAuthentication)
            {
                OrderModel model = new OrderModel();
                return View(model);
            }
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AlllActive]
        public ActionResult ToOrder(OrderModel model)
        {
            if (!Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else if (ModelState.IsValid)
            {

                Manager.ToOrder(model);

                model = new OrderModel();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AlllActive]
        public ActionResult Remove(Guid order_id)
        {
            Manager.RemoveOrder(order_id);
            return Redirect("index");
        }
    }
}