using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChudoPechkaLib
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
        }
    }
}
