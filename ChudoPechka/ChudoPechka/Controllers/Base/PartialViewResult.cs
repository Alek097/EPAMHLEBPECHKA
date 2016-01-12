using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChudoPechka.Controllers.Base
{
    public class PartialViewResult : System.Web.Mvc.PartialViewResult
    {
        private string _html;
        public PartialViewResult(string html)
        {
            this._html = html;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write(this._html);
        }
    }
}