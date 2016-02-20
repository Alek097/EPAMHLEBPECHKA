using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

using ChudoPechkaLib.Data;

namespace ChudoPechka
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
           IStoreDB db = DependencyResolver.Current.GetService<IStoreDB>();
            db.SaveChanges();
            db.Dispose();
        }
    }
}
