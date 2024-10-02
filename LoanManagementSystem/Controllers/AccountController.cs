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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login data.");
                return View(model);
            }

            var user = _accountService.GetUserByUsername(model.UserName);
            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError("", "Username/Password don't match.");
                return View(model);
            }

            // Validate role based on LoginType
            if (model.LoginType == "Employee" && user.Role.RoleName == "Customer" ||
                model.LoginType != "Employee" && (user.Role.RoleName == "Admin" || user.Role.RoleName == "LoanOfficer"))
            {
                ModelState.AddModelError("", "Username/Password don't match.");
                return View(model);
            }

            // Check password
            if (!BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Username/Password don't match.");
                return View(model);
            }

            // Role-specific redirects and session management
            switch (user.Role.RoleName)
            {
                case "Admin":
                    Session["Admin"] = _accountService.GetAdminByUserId(user.UserId);
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Admin");

                case "LoanOfficer":
                    Session["Officer"] = _accountService.GetOfficerByUserId(user.UserId);
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Welcome", "LoanOfficer");

                default: // Assuming Customer role
                    var customer = _accountService.GetCustomerByUserId(user.UserId);
                    Session["Customer"] = customer;
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("index", "customer");
            }
        }


        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            if (_accountService.CheckUserNameFound(customer.User.Username))
                ModelState.AddModelError("", "Username already exists! Choose a new Username");
            if (_accountService.CheckEmailFound(customer.User.Email))
                ModelState.AddModelError("", "Email already exists! Choose a new Email");
            if(!ModelState.IsValid)
                return View();
            _accountService.AddCustomer(customer);
            return RedirectToAction("Login");

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}

