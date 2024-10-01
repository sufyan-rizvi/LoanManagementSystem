using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using LoanManagementSystem.Controllers;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LoanManagementSystem.Models.Startup))]

namespace LoanManagementSystem.Models
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {//sufyan
            JobStorage.Current = new SqlServerStorage(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LoanDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            SchedulerController obSchedulerController = new SchedulerController();
            RecurringJob.AddOrUpdate(() => obSchedulerController.RunSchedulerMethod(), Cron.Minutely);
            app.UseHangfireServer();
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}