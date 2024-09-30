using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using System.Web.Security;
using LoanManagementSystem.ViewModels;
using NHibernate.Linq;
using LoanManagementSystem.Service;

namespace LoanManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View();
            var user = _accountService.GetUserByUsername(model.UserName);
            if (user == null)
                throw new InvalidOperationException("Username/ password does not match");
            if (model.LoginType == "Employee")
            {
                if (user.Role.RoleName == "Customer")
                    throw new InvalidOperationException("The username/ password does not match");
            }
            else
            {
                if (user.Role.RoleName == "Admin" || user.Role.RoleName == "LoanOfficer")
                    throw new InvalidOperationException("The username/ password does not match");
            }


            //string hashedPassword = HashingService.HashPassword(model.Password);
            bool isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.Password);
            if (isPasswordValid)
            {
                if (user.Role.RoleName == "Admin")
                {
                    Session["Admin"] = _accountService.GetAdminByUserId(user.UserId);
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role.RoleName == "LoanOfficer")
                {
                    Session["Officer"] = _accountService.GetOfficerByUserId(user.UserId);
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return RedirectToAction("Welcome", "LoanOfficer");
                }
                else
                {
                    Session["Customer"] = _accountService.GetCustomerByUserId(user.UserId);
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return RedirectToAction("Index", "Customer");
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
            _accountService.AddCustomer(customer);
            return RedirectToAction("Loginuser");

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Loginuser");
        }

    }
}

