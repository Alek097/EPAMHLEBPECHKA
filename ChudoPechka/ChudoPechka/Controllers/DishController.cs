using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class DishController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Dish
        public ActionResult Index(Guid dish_id)
        {
            Dish dish = null;

            if (Auth.GetDish(dish_id, out dish))
            {
                return View(dish);
            }
            else
                throw new HttpException(404, "Блюдо не найдено");
        }
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(string text, string user_login, Guid dish_id)
        {
            User usr = null;

            if (Auth.GetUser(user_login, out usr) && usr.Equals(Auth.User))
            {
                Auth.AddComment(user_login, text, dish_id);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(401, "Ошибка авторизации");
        }
    }
}