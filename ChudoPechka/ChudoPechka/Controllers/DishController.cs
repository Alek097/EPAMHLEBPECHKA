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

            if (Manager.GetDish(dish_id, out dish))
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

            if (Manager.GetUser(user_login, out usr) && usr.Equals(Manager.User))
            {
                Manager.AddComment(user_login, text, dish_id);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(401, "Ошибка авторизации");
        }
        [ValidateAntiForgeryToken]
        public ActionResult RemoveComment(string user_login, Guid comment_id)
        {
            User usr = null;
            Comment comment = null;
            if (Manager.GetUser(user_login, out usr) && usr.Equals(Manager.User))
            {
                if (Manager.GetComment(comment_id, out comment))
                {
                    if (comment.User.Equals(Manager.User))
                    {
                        try
                        {
                            Manager.RemoveComment(comment_id);
                            throw new HttpException(200, "OK");
                        }
                        catch (InvalidOperationException ex)
                        {
                            throw new HttpException(404, ex.Message);
                        }
                    }
                    else
                        throw new HttpException(423, "Доступ запрещён");
                }
                else
                    throw new HttpException(404, "Комментарий не найден");
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
            User usr = null;
            Comment comment = null;
            if (Manager.GetUser(user_login, out usr) && usr.Equals(Manager.User))
            {
                if (Manager.GetComment(comment_id, out comment))
                {
                    if (comment.User.Equals(Manager.User))
                    {
                        try
                        {
                            Manager.UpdateComment(comment_id, text);
                            throw new HttpException(200, "OK");
                        }
                        catch (InvalidOperationException ex)
                        {
                            throw new HttpException(404, ex.Message);
                        }
                    }
                    else
                        throw new HttpException(423, "Доступ запрещён");
                }
                else
                    throw new HttpException(404, "Комментарий не найден");
            }
            else
                throw new HttpException(401, "Ошибка авторизации");
        }
    }
}