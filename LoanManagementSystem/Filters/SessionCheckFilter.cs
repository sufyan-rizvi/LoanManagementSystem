using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace LoanManagementSystem.Filters
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        private readonly string[] _allowedUrls = new[]
        {
        "/Account/Login",      // Allow login page
        "/Account/Register",   // Allow register page
        "/Account/Logout",     // Allow logout page
        "/"  ,         // Allow customer-specific pages
        "/Customer/Schemes"           // Allow customer-specific pages
        // Add more URLs if needed
    };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            string requestUrl = httpContext.Request.Url.AbsolutePath;

            // Check if the request URL is in the allowed list
            if (Array.Exists(_allowedUrls, url => url.Equals(requestUrl, StringComparison.OrdinalIgnoreCase)))
            {
                base.OnActionExecuting(filterContext); // Allow request to proceed
                return;
            }

            // Check session variables for valid user roles
            if (httpContext.Session["Customer"] == null &&
                httpContext.Session["Admin"] == null &&
                httpContext.Session["LoanOfficer"] == null)
            {
                // If session is null and not an allowed URL, redirect to login
                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else
            {
                // If session exists, let the request proceed
                base.OnActionExecuting(filterContext);
            }
        }
    }

}