using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers
{
    public class SchedulerController : Controller
    {
        // GET: Scheduler
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> RunSchedulerMethod()
        {
            //SendEmail();
            return Json("Great Success!");
        }
        private async static void SendEmail()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Credentials = new NetworkCredential("dfgutdxvhuyf@gmail.com", "gpjb izjt kmfs tnzs");
            client.Timeout = 10000;
            client.Port = 587;
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("dfgutdxvhuyf@gmail.com");
            mailMessage.To.Add("nehalaiq@gmail.com");
            mailMessage.Subject = "Hello There";
            mailMessage.Body = "Hello Friend, Sufyan Here";
            client.Send(mailMessage);

        }
    }
}