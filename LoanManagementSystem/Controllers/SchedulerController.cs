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
            await SendEmail();
            await CheckNPA();
            return Json("Great Success!");
        }

        private async static Task CheckNPA()
        {
            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    try
                    {
                        var applications = _customerService.GetAllLoanApplications().Where(l=>l.Status == ApplicationStatus.LoanRepayment);

                        foreach (var application in applications)
                        {
                            int months = ((DateTime.Now.Year - application.PaymentStartDate.Year) * 12) + DateTime.Now.Month - application.PaymentStartDate.Month;

                            // Calculate missed payments
                            int missedPayments = Math.Max(0, months - application.PaymentsMade);
                            application.PaymentsMissed = missedPayments;

                            // Set status to NPA if missed payments are 3 or more
                            if (missedPayments >= 3 && application.Status != ApplicationStatus.NPA)
                            {
                                application.Status = ApplicationStatus.NPA;
                            }

                            // Merge application with the session
                            s.Merge(application);
                        }

                        // Commit the transaction after processing all applications
                        txn.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        txn.Rollback();
                        // Log the exception (consider using a logging framework)
                        Console.WriteLine($"Error processing loan applications: {ex.Message}");
                    }
                }
            }
        }

        private async static Task SendEmail()
        {
            var applications = _customerService.GetAllLoanApplications().Where(l =>
            {
                bool isWithinPaymentDateRange = DateTime.Now >= l.NextPaymentDate.AddDays(-1) && DateTime.Now <= l.NextPaymentDate.AddDays(4);

                var lastApprovedRepayment = l.Repayments.LastOrDefault(r => r.IsApproved);
                bool paymentAlreadyDone = lastApprovedRepayment != null && lastApprovedRepayment.PaymentDate >= l.NextPaymentDate.AddMonths(-1) &&
                                          lastApprovedRepayment.PaymentDate <= l.NextPaymentDate.AddMonths(-1).AddDays(5);

                return isWithinPaymentDateRange && !paymentAlreadyDone && l.Status == ApplicationStatus.LoanRepayment;
            }
            ).ToList();

            await SendEmailsAsync(applications);
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

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                    Subject = "Loan Repayment Reminder",
                    Body = $"<p>Dear {application.Applicant.User.FirstName},</p>" +
           "<p>This is a reminder that your loan repayment is due soon.</p>" +
           "<p>Please ensure that you make the payment on or before the due date to avoid any penalties.</p>" +
           "<p>If you have already made the payment, please disregard this message.</p>" +
           "<p>Thank you for your attention to this matter.</p>" +
           "<p>Best regards,<br/>Your Company Name</p>",
                    IsBodyHtml = true
                };

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