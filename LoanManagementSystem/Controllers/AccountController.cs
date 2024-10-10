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
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;

namespace LoanManagementSystem.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginVM model)
        {
            //bool isCaptchaValid = ValidateCaptcha(Request["g-recaptcha-response"]);
            bool isCaptchaValid = true;
            if (!isCaptchaValid)
            {
                ModelState.AddModelError("", "You have put wrong Captcha, Please ensure the authenticity!!");
                return View();
            }
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
                    var officer = _accountService.GetOfficerByUserId(user.UserId);
                    return RedirectToAction("Welcome", "LoanOfficer");

                default: // Assuming Customer role
                    var customer = _accountService.GetCustomerByUserId(user.UserId);
                    Session["Customer"] = customer;
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("index", "customer");
            }
        }
        [AllowAnonymous]
        private bool ValidateCaptcha(string response)
        {
            string secret = ConfigurationManager.AppSettings["GoogleSecretKey"];
            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
            return Convert.ToBoolean(captchaResponse.Success);
        }

        [HttpGet]
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public ActionResult Register(User User)
        {
            var customer = new Customer();
            customer.User = User;
            if (_accountService.CheckUserNameFound(customer.User.Username))
                ModelState.AddModelError("", "Username already exists! Choose a new Username");
            if (_accountService.CheckEmailFound(customer.User.Email))
                ModelState.AddModelError("", "Email already exists! Choose a new Email");
            if (!ModelState.IsValid)
                return View();
            _accountService.AddCustomer(customer);
            SendEmail(customer);
            return RedirectToAction("Login");

        }

        private static void SendEmail(Customer customer)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("dfgutdxvhuyf@gmail.com", "gpjb izjt kmfs tnzs");
                client.Timeout = 10000;
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                    Subject = "Welcome to Aksys Loans!",
                    Body = @"<style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }
            .container {
                background-color: #ffffff;
                margin: 0 auto;
                padding: 20px;
                max-width: 600px;
                border-radius: 8px;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }
            .header {
                background-color: #4CAF50;
                color: white;
                padding: 10px;
                text-align: center;
                border-radius: 8px 8px 0 0;
            }
            .content {
                padding: 20px;
            }
            .content h1 {
                font-size: 24px;
                color: #333;
            }
            .content p {
                font-size: 16px;
                line-height: 1.6;
                color: #555;
            }
            .content a {
                color: #4CAF50;
                text-decoration: none;
            }
            .footer {
                text-align: center;
                padding: 10px;
                font-size: 12px;
                color: #888;
            }
            .button {
                display: inline-block;
                background-color: #4CAF50;
                color: white;
                padding: 10px 20px;
                border-radius: 4px;
                text-decoration: none;
                margin-top: 20px;
            }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Welcome to Aksys Loans!</h1>
            </div>
            <div class='content'>" +
                $"<h1>Dear {customer.User.FirstName},</h1>" +
                @"<p>Thank you for registering with <strong>Aksys Loans</strong>. We are excited to have you on board! Your account has been successfully created, and you can now manage your loan applications, repayments, and other financial details seamlessly from your personalized dashboard.</p>
                <h3>Here's what you can do next:</h3>
                <ul>
                    <li><strong>Explore Loan Options:</strong> Browse through a variety of loan schemes that best fit your financial needs.</li>
                    <li><strong>Submit a Loan Application:</strong> Easily apply for loans by submitting the required documents and information.</li>
                    <li><strong>Track Your Loan Status:</strong> Stay updated on your loan application progress in real-time.</li>
                    <li><strong>Manage Payments:</strong> Schedule and track your loan repayments directly from your account.</li>
                </ul>
                <h3>Need Help?</h3>
                <p>Our support team is here to assist you with any questions or concerns you may have. You can reach us at <a href='mailto:support@aksysloans.com'>support@aksysloans.com</a> or call us at <strong>+123 456 7890</strong>.</p>
                <p>We look forward to serving your financial needs and ensuring a smooth loan management experience.</p>
                <p>Thank you for choosing <strong>Aksys Loans</strong>!</p>
            </div>
            <div class='footer'>
                <p>&copy; 2024 Aksys Loans. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(customer.User.Email);

                client.Send(mailMessage);
            }
        }

        [HttpGet]
        [Route("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }

    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ChallengeTimestamp { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; } // For reCAPTCHA v3
    }
}

