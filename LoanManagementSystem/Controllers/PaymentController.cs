using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Service;
using Razorpay.Api;
using Unity;


namespace LoanManagementSystem.Controllers
{
    [Authorize(Roles = "Customer")]

    public class PaymentController : Controller
    {
        private static ICustomerService _customerService = UnityConfig.container.Resolve<CustomerService>();
        // GET: RazorPayment
        private string _key = ConfigurationManager.AppSettings["RazorpayKey"];
        private string _secret = ConfigurationManager.AppSettings["RazorpaySecret"];

        // GET: Payment

        public ActionResult Index(string applicationId)
        {

            using (var s = NhibernateHelper.CreateSession())
            {
                var application = s.Query<LoanApplication>().FirstOrDefault(l => l.ApplicationId == new Guid(applicationId));
                Session["application"] = application;
                return View(application);

            }
        }
        // Create Razorpay Order
        [HttpGet]
        public JsonResult CreateOrder(string amount)
        {
            try
            {
                Session["amount"] = amount;
                var client = new RazorpayClient(_key, _secret);

                // Convert the amount to paise (smallest currency unit)
                int amountInCents = Convert.ToInt32(Convert.ToDecimal(amount) * 100);

                // Create an order in Razorpay for the amount entered by the user
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", amountInCents }, // Amount in paise
                    { "currency", "INR" },
                    { "receipt", "order_rcptid_11" }
                };

                Order order = client.Order.Create(options);

                // Return the order ID and amount to the client-side
                return Json(new { orderId = order["id"].ToString(), amount = amountInCents }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Payment/ProcessPayment
        [HttpPost]
        public ActionResult ProcessPayment(string rzp_payment_id, string rzp_order_id, string rzp_signature)
        {
            try
            {
                Session["rzp_order_id"] = rzp_order_id;
                var client = new RazorpayClient(_key, _secret);

                // Create the parameters for payment verification
                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "razorpay_payment_id", rzp_payment_id },
                    { "razorpay_order_id", rzp_order_id },
                    { "razorpay_signature", rzp_signature }
                };

                // Verify the payment signature
                Utils.verifyPaymentSignature(attributes);

                // Redirect to success page if the payment is successful
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Success()
        {

            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var payment = new Repayment
                    {
                        PaymentDate = DateTime.Now,
                        Amount = Convert.ToDouble(Session["amount"]),
                        TransactionId = (string)Session["rzp_order_id"],
                        IsApproved = true,
                        Application = (LoanApplication)Session["application"]

                    };
                    s.Save(payment);
                    txn.Commit();
                }

            }

            return View();
        }


        public ActionResult Error()
        {
            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var payment = new Repayment
                    {
                        PaymentDate = DateTime.Now,
                        Amount = (double)Session["amount"],
                        TransactionId = (string)Session["rzp_order_id"],
                        IsApproved = false
                    };
                    s.Save(payment);
                    txn.Commit();
                }

            }
            return View();
        }
        public ActionResult UpdateApplicationAfterPayment()
        {

            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var application = (LoanApplication)Session["application"];
                    application.PaymentsMade += 1;
                    if (application.PaymentsMade >= application.Tenure)
                    {
                        application.Status = ApplicationStatus.LoanClosed;
                        LoanClosureMail(application);
                    }

                    application.EMIPaymentMade = true;
                    application.PaymentsMissed = 0;
                    application.NextPaymentDate = application.NextPaymentDate.AddMonths(1);

                    s.Merge(application);
                    txn.Commit();
                    SuccessPaymentEmail(application);


                }
            }
            return Json("YAY", JsonRequestBehavior.AllowGet);
        }

        private static void SuccessPaymentEmail(LoanApplication applicationId)
        {
            var loanApplication = _customerService.GetApplicationById(applicationId.ApplicationId);
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("dfgutdxvhuyf@gmail.com", "gpjb izjt kmfs tnzs");
                client.Timeout = 10000;
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                    Subject = "Loan Repayment Successful!",
                    Body = @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
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
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Loan Repayment Successful!</h1>
                </div>
                <div class='content'>
                    <h1>Dear " + loanApplication.Applicant.User.FirstName + @",</h1>
                    <p>We are pleased to inform you that your recent loan repayment has been successfully processed. Thank you for your timely payment!</p>
                    <p><strong>Payment Details:</strong></p>
                    <ul>
                        <li><strong>Loan Amount:</strong> $" + loanApplication.LoanAmount + @"</li>
                        <li><strong>Repayment Amount:</strong> $" + loanApplication.EMIAmount.ToString("C", new CultureInfo("en-IN")) + @"</li>
                        <li><strong>Payment Date:</strong> " + DateTime.Now.ToString("dd/MM/yyyy") + @"</li>
                    </ul>
                    <h3>What's Next?</h3>
                    <p>Your next payment is scheduled for <strong>" + loanApplication.NextPaymentDate.ToString("dd/MM/yyyy") + @"</strong>. You can always log in to your <a href='#'>Aksys Loans</a> account to view your repayment schedule, track your loan status, and manage your account.</p>
                    <p>Thank you for staying on top of your repayments and for choosing <strong>Aksys Loans</strong> to manage your financial needs. We appreciate your trust in us.</p>
                    <h3>Need Help?</h3>
                    <p>If you have any questions, feel free to contact our support team at <a href='mailto:support@aksysloans.com'>support@aksysloans.com</a> or call us at <strong>+123 456 7890</strong>.</p>
                    <p>We look forward to continuing to assist you with your financial needs.</p>
                    <p>Thank you,</p>
                    <p><strong>The Aksys Loans Team</strong></p>
                </div>
                <div class='footer'>
                    <p>&copy; 2024 Aksys Loans. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>",
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };
                mailMessage.To.Add(loanApplication.Applicant.User.Email);

                client.Send(mailMessage);
            }
        }

        private static void LoanClosureMail(LoanApplication application)
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
                    Subject = "Loan Closure Confirmation!",
                    Body = @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
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
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Loan Closure Confirmation!</h1>
                </div>
                <div class='content'>
                    <h1>Dear " + application.Applicant.User.FirstName + @",</h1>
                    <p>We are pleased to inform you that your loan has been successfully closed. Thank you for your timely payments and for choosing <strong>Aksys Loans</strong> for your financial needs!</p>
                    <p><strong>Loan Details:</strong></p>
                    <ul>
                        <li><strong>Loan Amount:</strong> $" + application.LoanAmount.ToString("C", new CultureInfo("en-IN")) + @"</li>
                        <li><strong>Final Payment Date:</strong> " + DateTime.Now.ToString("dd/MM/yyyy") + @"</li>
                        <li><strong>Status:</strong> Closed</li>
                    </ul>
                    <h3>What’s Next?</h3>
                    <p>Your account will remain accessible for you to view your loan history and any related documents. If you require any further assistance or wish to explore more financial products, please don’t hesitate to reach out to us.</p>
                    <h3>Need Help?</h3>
                    <p>If you have any questions, feel free to contact our support team at <a href='mailto:support@aksysloans.com'>support@aksysloans.com</a> or call us at <strong>+123 456 7890</strong>.</p>
                    <p>Thank you for being a valued customer, and we look forward to serving you in the future!</p>
                    <p>Best Regards,</p>
                    <p><strong>The Aksys Loans Team</strong></p>
                </div>
                <div class='footer'>
                    <p>&copy; 2024 Aksys Loans. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>",
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };
                mailMessage.To.Add(application.Applicant.User.Email);

                client.Send(mailMessage);
            }

        }


    }
}