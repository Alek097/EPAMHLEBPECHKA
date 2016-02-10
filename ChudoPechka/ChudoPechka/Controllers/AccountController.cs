using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;
using ChudoPechkaLib.Data;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class AccountController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            return View();//TODO: Написать представление описания юзера
        }
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
                Auth.RegisterUser(model);
                return Redirect(Url.Action("LoginIn"));
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
            if (!Auth.IsAuthentication)
            {
                if (ModelState.IsValid && Auth.LoginIn(model.Login, model.Password))
                    return Redirect(Url.Action("Index", "Home"));

                ModelState.AddModelError("Login", "Неверный логин или пароль");

                return View(model);
            }
            else
                return Redirect(Url.Action("Index", "Home"));
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
        [HttpGet]
        public ActionResult GetUserToRecovery(string login)
        {
            User usr;
            if (Auth.GetUser(login, out usr))
                return View(usr);
            else
                return new ChudoPechka.Controllers.Base.PartialViewResult("Пользователь с логином не найден");

        }
        public ActionResult GetUser(string login)
        {
            User usr;
            if (Auth.GetUser(login, out usr))
                return View(usr);
            else
                return new ChudoPechka.Controllers.Base.PartialViewResult("Пользователь с логином не найден");
        }
    }
}