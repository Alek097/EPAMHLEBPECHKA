﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Filters;

namespace ChudoPechka.Controllers
{
    public class HomeController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Home
        [AlllActive]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetMenu()
        {
            return PartialView();
        }
    }
}