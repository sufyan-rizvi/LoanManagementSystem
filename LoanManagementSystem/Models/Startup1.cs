using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hangfire;
using Hangfire.SqlServer;
using LoanManagementSystem.Controllers;
using LoanManagementSystem.Service;
using Microsoft.Owin;
using Owin;
using Unity;

[assembly: OwinStartup(typeof(LoanManagementSystem.Models.Startup))]

namespace LoanManagementSystem.Models
{
    [AllowAnonymous]
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JobStorage.Current = new SqlServerStorage(@"Data Source=DESKTOP-646UVFV\SQLEXPRESS;Initial Catalog=LoanDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            SchedulerController obSchedulerController = new SchedulerController();
            RecurringJob.AddOrUpdate(() => obSchedulerController.RunSchedulerMethod(), Cron.Minutely());
            app.UseHangfireServer();
            app.UseHangfireDashboard();

        }
    }
}