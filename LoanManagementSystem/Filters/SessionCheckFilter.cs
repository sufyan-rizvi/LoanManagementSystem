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
        "/Customer/Schemes",  // Allow login page
        "/Account/Register",  
        "/Account/Login"// Allow home page
        // Add more URLs as needed
    };
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var httpContext = filterContext.HttpContext;

            // Check if the request URL is in the allowed list
            string requestUrl = httpContext.Request.Url.AbsolutePath;
            if (Array.Exists(_allowedUrls, url => url.Equals(requestUrl, StringComparison.OrdinalIgnoreCase)))
            {
                return; // Allow the request to go through
            }

            // Check session variables
            if (httpContext.Session["Customer"] == null &&
                httpContext.Session["Admin"] == null &&
                httpContext.Session["LoanOfficer"] == null)
            {
                // Redirect to login page if all session variables are null
                filterContext.Result = new RedirectResult("/Account/Login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}