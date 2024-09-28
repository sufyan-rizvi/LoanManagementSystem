using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using LoanManagementSystem.Models;
using Quartz;

namespace LoanManagementSystem.Jobs
{
    public class RepaymentEmailSendingJob : IJob
    {
        private readonly LoanApplication _application;
        public RepaymentEmailSendingJob(LoanApplication application)
        {
            _application = application;
        }
        public Task Execute(IJobExecutionContext context)
        {
            SendEmail();
            return Task.CompletedTask;
        }
        private static void SendEmail()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Credentials = new NetworkCredential("sofylaiq@gmail.com", "mquj vkif zzil ljcw");
            client.Timeout = 10000;
            client.Port = 587;
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sofylaiq@gmail.com");
            mailMessage.To.Add("nehalaiq@gmail.com");
            mailMessage.Subject = "Hello There";
            mailMessage.Body = "Hello Friend";
            client.Send(mailMessage);

        }
    }
}