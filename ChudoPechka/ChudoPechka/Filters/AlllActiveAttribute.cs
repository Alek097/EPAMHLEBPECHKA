using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


using ChudoPechkaLib;

namespace ChudoPechka.Filters
{
    public class AlllActiveAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IDBManager manager = DependencyResolver.Current.GetService<IDBManager>();
            
            if(manager.IsAuthentication && !manager.User.IsActive)
            {
                filterContext.Result = new RedirectResult(string.Format("/Account/Confirm?login={0}&e_mail={1}", manager.User.Login, manager.User.E_Mail));
            }
        }
    }
}