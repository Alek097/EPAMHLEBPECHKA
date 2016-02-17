using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;

namespace ChudoPechka.Controllers
{
    public class AnnouncedController : ChudoPechka.Controllers.Base.BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public void SendAnnounced(AnnouncedModel model)
        {
            if (Auth.IsAuthentication)
                Auth.SendAnnounced(model);
        }
    }
}