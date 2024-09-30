using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace LoanManagementSystem.Filters
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if all session values are null
            if (HttpContext.Current.Session["Customer"] == null &&
                HttpContext.Current.Session["Admin"] == null &&
                HttpContext.Current.Session["LoanOfficer"] == null)
            {
                var isLoginPage = filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/Account/Login");

                if (!isLoginPage)
                {
                    // Clear the session
                    HttpContext.Current.Session.Clear();

                    // Redirect to the login page
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}