﻿using System;
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
            return RedirectToAction("LoginUser");

            
        }

           
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
                    string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password,6);
                    user.Password = hashedPassword;
                    user.IsActive = true;
                    user.Role = new Role()
                    {
                        RoleName = "Customer",
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
            return RedirectToAction("Loginuser");
        }

    }
}

