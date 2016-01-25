using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChudoPechka.Controllers
{
    public class HomeController : ChudoPechka.Controllers.Base.BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMenu()
        {

            ChudoPechkaLib.Menu.Menu menu = (ChudoPechkaLib.Menu.Menu)System.Web.Mvc.DependencyResolver.Current.GetService<ChudoPechkaLib.Menu.IMenu>();
            string HTML = null;

            foreach (ChudoPechkaLib.Menu.MenuItem item in menu.MenuItems)
            {
                HTML += "<div class=\"menuItem\">" +
                    "<div class=\"head\">"
                    +"<hr size=\"5\" color=\"black\" width=\"200\" />"
                    + "<span class=\"dayName\">"+ item.Day + "</span>"+
                    "<hr size=\"5\" color=\"black\" width=\"200\" />"+
                    "</div>"+
                    "<div class=\"imgConteiner\">"
                    + item.Img +
                    "</div>" 
                    + item.Menu + 
                    "</div>";
            }
            return new ChudoPechka.Controllers.Base.PartialViewResult(HTML);
        }
    }
}