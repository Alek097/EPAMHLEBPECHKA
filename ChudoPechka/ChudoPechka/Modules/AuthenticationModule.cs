using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using ChudoPechkaLib;
using ChudoPechkaLib.Data;

namespace ChudoPechka.Modules
{
    class AuthenticationModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.Authenticate);
        }

        private void Authenticate(Object source, EventArgs e)
        {
            HttpContext context = (source as HttpApplication).Context;
            IDBManager auth = DependencyResolver.Current.GetService<IDBManager>();
            auth.Start(context,
                DependencyResolver.Current.GetService<IStoreDB>());
        }
    }
}
