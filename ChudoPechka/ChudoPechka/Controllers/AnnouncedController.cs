using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class AnnouncedController : ChudoPechka.Controllers.Base.BaseController
    {
        public ActionResult Index()
        {
            if (!Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }
        public ActionResult GetAnnounced()
        {
            if (!Auth.IsAuthentication)
                throw new HttpException(423,"Вы не авторизованы");
            else
            {
                List<Announced> Anns = Auth.User.Announceds.ToList();
                return View(Anns);
            }
        }
        [ValidateAntiForgeryToken]
        public void SendAnnounced(AnnouncedModel model)
        {
            if (Auth.IsAuthentication)
                Auth.SendAnnounced(model);
        }
    }
}