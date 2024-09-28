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
        
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                using (var session = NhibernateHelper.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var user = session.Query<User>().SingleOrDefault(u => u.Username == model.UserName);
                        if (user != null)
                        {
                            //string hashedPassword = HashingService.HashPassword(model.Password);
                            bool isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.Password);
                            if (isPasswordValid)
                            {
                                if (user.Role.RoleName == "Admin")
                                {
                                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                                    return RedirectToAction("Index", "Admin");
                                }
                                else if(user.Role.RoleName == "LoanOfficer")
                                {
                                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                                    return RedirectToAction("Welcome", "LoanOfficer");
                                }
                                else
                                {
                                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                                    return RedirectToAction("ListSchemes", "LoanScheme");
                                }
                                
                                

                            }
                            
                        }
                        else
                        {
                            return RedirectToAction("LoginUser");
                        }
                    }
                }

            }
            return View();

            
        }

           
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    customer.User.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(customer.User.Password, 6);
                    customer.User.IsActive = true;
                    customer.User.Role = new Role();
                    customer.User.Role.RoleName = "Customer";
                    customer.User.Role.User = customer.User;

                    session.Save(customer);
                    transaction.Commit();
                    return RedirectToAction("Loginuser");
                }
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Loginuser");
        }

    }
}

