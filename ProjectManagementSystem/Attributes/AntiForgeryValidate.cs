using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementSystem.Attributes
{
    public class AntiForgeryValidate : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cookieToken = "";
            string formToken = "";
            
            if (HttpContext.Current.Request.Headers.AllKeys.Any(x=> x == "RequestVerificationToken"))
            {
                string[] tokens = HttpContext.Current.Request.Headers["RequestVerificationToken"].Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            System.Web.Helpers.AntiForgery.Validate(cookieToken, formToken);
            base.OnActionExecuting(filterContext);
        }
    }
}