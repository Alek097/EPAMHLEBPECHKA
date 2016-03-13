using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Filters;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class DishController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Dish
        [AlllActive]
        public ActionResult Index(Guid dish_id)
        {
            Dish dish = null;

            if (Manager.GetDish(dish_id, out dish))
            {
                return View(dish);
            }
            else
                throw new HttpException(404, "Блюдо не найдено");
        }
        [ValidateAntiForgeryToken]
        [AlllActive]
        public ActionResult AddComment(string text, string user_login, int ball, Guid dish_id)
        {
            User usr = null;

            if (Manager.GetUser(user_login, out usr) && usr.Equals(Manager.User))
            {
                Manager.AddComment(usr, text,ball ,dish_id);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(401, "Ошибка авторизации");
        }
        [ValidateAntiForgeryToken]

        public ActionResult RemoveComment(string user_login, Guid comment_id)
        {
            if (Manager.IsAuthentication)
            {
                Manager.RemoveComment(comment_id);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(401, "Ошибка авторизации");
        }
        public PartialViewResult GetComments(Guid dish_id)
        {
            Dish dish = null;
            if (Manager.GetDish(dish_id, out dish))
            {
                return PartialView(dish.Comments.ToList());
            }
            else
                throw new HttpException(404, "Блюдо не найдено");
        }
        [ValidateAntiForgeryToken]
        public ActionResult UpdateComment(Guid comment_id, string user_login, string text)
        {
            if (Manager.IsAuthentication && Manager.User.Login.Equals(user_login))
            {
                Manager.UpdateComment(comment_id, text);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(401, "Ошибка авторизации");
        }

        public string GetRating(Guid dish_id)
        {
            Dish dish = null;

            if (Manager.GetDish(dish_id, out dish))
                return string.Format("{0:N}", dish.Rating);
            else
                throw new HttpException(404, "Блюдо не найден");
        }
    }
}