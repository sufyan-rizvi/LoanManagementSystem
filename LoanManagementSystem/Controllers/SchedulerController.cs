using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Service;
using LoanManagementSystem.Models;
using Razorpay.Api;
using static System.Net.Mime.MediaTypeNames;
using Unity;
using CloudinaryDotNet.Actions;

namespace LoanManagementSystem.Controllers
{
    [AllowAnonymous]
    public class SchedulerController : Controller
    {

        private static ICustomerService _customerService = UnityConfig.container.Resolve<CustomerService>();

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> RunSchedulerMethod()
        {
            SetPaymentMadeStatus();
            SendEmail();
            PaymentDateMissed();
            
           
            return Json("Great Success!");
        }

        private async static Task PaymentDateMissed()
        {
            var applications = _customerService.PaymentMissedApplications();
            _customerService.UpdateNextPaymentDate(applications);

        }

        private async static Task CheckNPA()
        {
            var applications = _customerService.GetNPAApplications();
            _customerService.CheckNPA(applications);
        }


        private async static Task SetPaymentMadeStatus()
        {

            var applications = _customerService.ApplicationsToBeMadeFalse();
            _customerService.SetApplicationsToFalse(applications);
        }

        private async static Task SendEmail()
        {            
            var emailsToSend = _customerService.ApplicationsWithEmailDue();            

            await SendEmailsAsync(emailsToSend);
        }


        public async static Task SendEmailsAsync(IList<LoanApplication> applications)
        {
            List<Task> emailTasks = new List<Task>();

            foreach (var application in applications)
            {
                var email = application.Applicant.User.Email;
                emailTasks.Add(SendEmailToUserAsync(email, application));
            }

            // Await all email sending tasks
            await Task.WhenAll(emailTasks);
        }

        public async static Task SendEmailToUserAsync(string email, LoanApplication application)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("dfgutdxvhuyf@gmail.com", "gpjb izjt kmfs tnzs");
                client.Timeout = 10000;
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mailMessage;

                if (DateTime.Now.Day < application.NextPaymentDate.Day)
                {
                    mailMessage = new MailMessage
                    {
                        From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                        Subject = "Loan Repayment Reminder",
                        Body = $"<p>Dear {application.Applicant.User.FirstName},</p>" +
           $"<p>This is a reminder that your loan repayment with the Id: {application.ApplicationId} is due soon.</p>" +
           "<p>Please ensure that you make the payment on or before the due date to avoid any penalties.</p>" +
           "<p>If you have already made the payment, please disregard this message.</p>" +
           "<p>Thank you for your attention to this matter.</p>" +
           "<p> Best regards,<br/>" +
            "Aksys Loan Pvt.Ltd.</p>" +
            "<p> &copy; 2024 Aksys Loan Pvt.Ltd." +
            "</br>All rights reserved.</p>",
                        IsBodyHtml = true
                    };
                }
                else
                {
                    mailMessage = new MailMessage
                    {
                        From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                        Subject = "Loan Repayment Reminder",
                        Body = $"<p>Dear {application.Applicant.User.FirstName},</p>" +
                               $"<p>This is a reminder that your loan repayment with the Id: {application.ApplicationId} is due now {DateTime.Now.Date}.</p>" +
                               $"<a href=\"/Payment/Index?applicationId={application.ApplicationId}\" class=\"btn btn-outline-danger\" style=\"width: 150px; border: none; padding: 10px; color: white; background-color: #A6133C; border-radius: 20px; border-right-color: none;\">\r\n    Pay Now!\r\n</a>" +
                               "<p>Please ensure that you make the payment on or before the due date to avoid any penalties.</p>" +
                               "<p>If you have already made the payment, please disregard this message.</p>" +
                               "<p>Thank you for your attention to this matter.</p>" +
                               "Aksys Loan Pvt.Ltd.</p>" +
            "<p> &copy; 2024 Aksys Loan Pvt.Ltd.</p>" +
            "<p>All rights reserved.</p>",
                        IsBodyHtml = true
                    };
                }


                mailMessage.To.Add(email);

                try
                {
                    // Send email asynchronously
                    await client.SendMailAsync(mailMessage);
                    //Console.WriteLine($"Email successfully sent to {email}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
                }
            }
        }
    }
}