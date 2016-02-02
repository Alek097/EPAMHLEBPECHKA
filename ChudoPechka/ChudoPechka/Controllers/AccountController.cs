using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;
using ChudoPechkaLib.Data;
using ChudoPechkaLib.Data.Model;

using ChudoPechkaLib.Data;

namespace ChudoPechka.Controllers
{
    public class AccountController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            else
            {
                Auth.RegisterUser(
                    model.Login,
                    model.Password,
                    model.FirsName,
                    model.SecondName,
                    model.SecretQuestion,
                    model.ResponseQuestion,
                    model.BirthDay);
                return Redirect(Url.Action("Index", "Home"));
            }
        }
        [HttpGet]
        public ActionResult LoginIn()
        {
            if (Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }
        [HttpPost]
        public ActionResult LoginIn(LoginModel model)
        {
            if (ModelState.IsValid && Auth.LoginIn(model.Login, model.Password))
                return Redirect(Url.Action("Index", "Home"));

            ModelState.AddModelError("Login", "Неверный логин или пароль");

            return View(model);
        }
        public ActionResult LoginOut()
        {
            Auth.LoginOut();
            return Redirect(Url.Action("Index", "Home"));
        }
        [HttpGet]
        public ActionResult Recovery()
        {
            if (Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }
        [HttpPost]
        public ActionResult Recovery(RecoveryModel model)
        {
            if (Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            try
            {
                if (!ModelState.IsValid)
                    return View();
                else if (Auth.UpdatePassword(model.login, model.newPass, model.responseQuestion))
                    return Redirect(Url.Action("LoginIn"));

                else
                    ModelState.AddModelError("", "Неверный ответ, попробуйте ещё.");
            }
            catch (NullReferenceException)
            {
                ModelState.AddModelError("", "Пользователь не найден, попробуйте ещё.");
            }
            return View();
        }
/*        [HttpGet]
        public ActionResult GetUser(string login)
        {
            using (UsersStoreDB db = new UsersStoreDB())
            {
                if (db.HasUser(login))
                {
                    User usr = db.Get_User(login);
                    return View(usr);
                }

                else
                    return new ChudoPechka.Controllers.Base.PartialViewResult("Пользователь с логином не найден");
            }

        }*/
    }
}