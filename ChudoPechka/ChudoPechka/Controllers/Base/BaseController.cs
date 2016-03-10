using System;
using System.Collections.Generic;
using ChudoPechkaLib.Menu;
using System.Web;
using System.Web.Mvc;

using ChudoPechkaLib;

namespace ChudoPechka.Controllers.Base
{
    public class BaseController : Controller
    {
        public IDBManager Manager { get; set; }
        public BaseController()
        {
            this.Manager = DependencyResolver.Current.GetService<IDBManager>();
        }
    }
}