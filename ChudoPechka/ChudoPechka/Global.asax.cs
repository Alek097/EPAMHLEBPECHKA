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

            string connectionString = WebConfigurationManager.ConnectionStrings["Store"].ConnectionString;

            StoreDB.ConnectionString = connectionString;
        }

        protected void Application_End()
        {
        }
    }
}
