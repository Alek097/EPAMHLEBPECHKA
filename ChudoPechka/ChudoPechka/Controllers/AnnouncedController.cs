using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Filters;
using ChudoPechka.Models;
using ChudoPechkaLib.Models;


namespace ChudoPechka.Controllers
{
    public class AnnouncedController : ChudoPechka.Controllers.Base.BaseController
    {
        [AlllActive]
        public ActionResult Index()
        {
            if (!Manager.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else
                return View();
        }
        [AlllActive]
        public PartialViewResult GetAnnounced()
        {
            if (!Manager.IsAuthentication)
                throw new HttpException(423, "Вы не авторизованы");
            else
            {
                List<Announced> Anns = Manager.User.Announceds.ToList();
                return PartialView(Anns);
            }
        }
        [ValidateAntiForgeryToken]
        [AlllActive]
        public void SendAnnounced(AnnouncedModel model)
        {
            if (Manager.IsAuthentication)
            {
                Manager.SendAnnounced(model);
                throw new HttpException(200, "OK");
            }
            else
                throw new HttpException(423, "Вы не авторизованы");
        }
    }
}