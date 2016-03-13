using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

using ChudoPechka.Filters;
using ChudoPechka.Models;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class AccountController : ChudoPechka.Controllers.Base.BaseController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferMoney(string from, string to, uint money = 0)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");

            Manager.TransferMoney(from, to, money);

            return Redirect(Url.Action("Index", new { login = from }));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMoney(string login, uint addMoney = 0)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");

            Manager.AddMoney(login, addMoney);

            return Redirect(Url.Action("Index", new { login = login }));
        }
        [HttpGet]
        [AlllActive]
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
        public async Task<ActionResult> Create(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            else
            {
                Manager.RegisterUser(model);
                await Manager.SendConfirmCodeAsync(model.Login, model.E_Mail);//Отправляем письмецо на подтверждение
                Manager.LoginIn(model.Login, model.Password);//Логинимся
                return Redirect(Url.Action("Confirm", new { e_mail = model.E_Mail, login = model.Login }));
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
        public ActionResult SelectUserToRecovery()
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
        [AlllActive]
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
                throw new HttpException(401, "Вы не авторизованы");
        }
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendConfirmCode(string e_mail, string login)
        {
            if (e_mail == null || login == null)
                throw new HttpException(400, "Bad Request");

            await Manager.SendConfirmCodeAsync(login, e_mail);

            if (!Manager.IsAuthentication)//Если мы вошли значит мы восстанавливаем пароль
                throw new HttpException(200, "OK");
            else
                return Redirect(Url.Action("Confirm", new { e_mail = e_mail, login = login }));
        }
        [HttpGet]
        public ActionResult Confirm(string e_mail, string login)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");
            else if (e_mail == null || login == null)
                throw new HttpException(400, "Bad Request");
            else if (!Manager.User.Login.Equals(login))
                throw new HttpException(423, "Ошибка доступа");

            return View(new ConfirmModel { E_Mail = e_mail, Login = login });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ConfirmModel model)
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(401, "Вы не авторизованы");
            else if (!Manager.User.Login.Equals(model.Login))
                throw new HttpException(423, "Ошибка доступа");
            else if (!Manager.User.ActivationCode.Equals(model.Code))
            {
                ModelState.AddModelError("", "Коды не совпадают");
                return View(model);
            }
            else if(!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                Manager.SetActiveCode(Manager.User);
                return Redirect(Url.Action("Index", "Home"));
            }
        }
        public ActionResult Recovery(string login, string e_mail)
        {
            if(Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));

            RecoveryModel model = new RecoveryModel();
            model.Login = login;
            model.E_Mail = e_mail;

            return View(model);
        }
        [HttpPost]
        public ActionResult Recovery(RecoveryModel model)
        {
            User user = null;
            if (Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else if (!ModelState.IsValid)
                return View(model);
            else if (!Manager.GetUser(model.Login, out user))
                throw new HttpException(404, "Пользователь не найден");
            else if (!user.ActivationCode.Equals(model.Code))
            {
                ModelState.AddModelError("", "Коды не совпадают");
                return View(model);
            }
            else
            {
                Manager.UpdatePassword(model.Login, model.newPass);
                return Redirect(Url.Action("LoginIn"));
            }
        }
    }
}