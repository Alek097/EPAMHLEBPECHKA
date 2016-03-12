using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

using ChudoPechka.Models;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class AccountController : ChudoPechka.Controllers.Base.BaseController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferMoney(string from, string to, uint money)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");

            Manager.TransferMoney(from, to, money);

            return Redirect(Url.Action("Index", new { login = from }));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMoney(string login, uint addMoney)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");

            Manager.AddMoney(login, addMoney);

            return Redirect(Url.Action("Index", new { login = login }));
        }
        // GET: Account
        [HttpGet]
        public ActionResult Index(string login)
        {
            User usr;
            if (Manager.GetUser(login, out usr))
                return View(usr);
            else
                throw new HttpException(404, "Пользователь не найден");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            else
            {
                Manager.RegisterUser(model);
                return Redirect(Url.Action("LoginIn"));
            }
        }
        [HttpGet]
        public ActionResult LoginIn()
        {
            if (Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginIn(LoginModel model)
        {
            if (!Manager.IsAuthentication)
            {
                if (ModelState.IsValid && Manager.LoginIn(model.Login, model.Password))
                    return Redirect(Url.Action("Index", "Home"));

                ModelState.AddModelError("Login", "Неверный логин или пароль");

                return View(model);
            }
            else
                return Redirect(Url.Action("Index", "Home"));
        }
        public ActionResult LoginOut()
        {
            Manager.LoginOut();
            return Redirect(Url.Action("Index", "Home"));
        }
        [HttpGet]
        public ActionResult Recovery()
        {
            if (Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }
        public PartialViewResult GetUser(string login)
        {
            User usr;
            if (Manager.GetUser(login, out usr))
                return PartialView(usr);
            throw new HttpException(404, "Пользователь не найден");
        }
        public PartialViewResult GetUsers(string e_mail)
        {
            List<User> users = null;
            if (Manager.GetUsersForEmail(e_mail, out users))
                return PartialView(users);
            else throw new HttpException(404, "Пользователи не найдены");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadAvatar(HttpPostedFileBase upload)
        {
            if (Manager.IsAuthentication)
            {
                if (upload != null)
                {
                    string UrlAvatar = Path.Combine("~", "img", "Users", Manager.User.Login + Path.GetExtension(upload.FileName));
                    string fileName = Server.MapPath(Path.Combine("~", "img", "Users", Manager.User.Login + Path.GetExtension(upload.FileName)));//Получаем путь к файлу

                    if (System.IO.File.Exists(fileName))//Проверяем есть ли такой файл
                        System.IO.File.Delete(fileName);//И удаляем его

                    Bitmap avatar = new Bitmap(Image.FromStream(upload.InputStream));
                    int newSize = 0;

                    if (avatar.Width > avatar.Height)//Изменяем размеры, не хотел делать отдельный приватный метод
                        newSize = avatar.Height;
                    else
                        newSize = avatar.Width;

                    Image newSizeAvatar = new Bitmap(newSize, newSize);
                    using (Graphics g = Graphics.FromImage((Image)newSizeAvatar))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(avatar, 0, 0, newSize, newSize);
                        g.Dispose();
                    }

                    newSizeAvatar.Save(fileName);

                    Manager.UpdateAvatar(Manager.User, UrlAvatar);

                    return Redirect(Url.Action("Index", new { login = Manager.User.Login }));
                }
                else
                    throw new HttpException(400, "Вы не выбрали файл");
            }
            else
                throw new HttpException(401, "Вы не авторизированы");


        }
    }
}