using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;

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
                UsersStoreDB.Add_User(
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
    }
}