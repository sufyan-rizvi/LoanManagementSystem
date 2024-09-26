using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using System.Web.Security;
using LoanManagementSystem.ViewModels;

namespace LoanManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return Content("Will redirect to Role Based Controller");
        }
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public JsonResult LoginUser(LoginVM model)
        {
            string message;
            bool isAuthenticated = false;

            if (ModelState.IsValid)
            {
                //using (var session = NhibernateHelper.CreateSession())
                //{
                //    using (var transaction = session.BeginTransaction())
                //    {
                //        var user = session.Query<User>().SingleOrDefault(u => u.Username == model.UserName);
                //        if (user != null)
                //        {
                //            string hashedPassword = HashingService.HashPassword(model.Password);

                //            if (user.Password == hashedPassword)
                //            {
                //                FormsAuthentication.SetAuthCookie(model.UserName, true);
                //                return Json(new { success = true });

                //            }
                //            else
                //            {
                //                message = "Invalid credentials.";
                //                return Json(new { success = false, message = message });
                //            }
                //        }
                //        else
                //        {
                //            message = "Invalid credentials.";
                //            return Json(new { success = false, message = message });
                //        }
                //    }
                //}



                if (model.LoginType == "User")
                {
                    // Add your User login logic here
                    if (model.UserName == "u" && model.Password == "p") // Dummy check
                    {
                        isAuthenticated = true;
                    }
                    else
                    {
                        message = "Invalid User credentials.";
                        return Json(new { success = false, message = message });
                    }
                }
                else if (model.LoginType == "Employee")
                {
                    // Add your Employee login logic here
                    if (model.UserName == "employee1" && model.Password == "password456") // Dummy check
                    {
                        isAuthenticated = true;
                    }
                    else
                    {
                        message = "Invalid Employee credentials.";
                        return Json(new { success = false, message = message });
                    }
                }
            }

            if (isAuthenticated)
            {
                // Redirect to Home page if login is successful
                return Json(new { success = true });
            }
            else
            {
                message = "Invalid login attempt.";
                return Json(new { success = false, message = message });
            }
        }
        //else
        //{
        //    message = "Invalid login attempt.";
        //    return Json(new { success = false, message = message });
        //}

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    string hashedPassword = HashingService.HashPassword(user.Password);
                    user.Password = hashedPassword;
                    user.IsActive = true;
                    user.Role = new Role()
                    {
                        RoleName = "customer",
                        User = user,
                    };
                    //user.Role.User = user;


                    session.Save(user);
                    transaction.Commit();
                    return RedirectToAction("Login");
                }
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}

