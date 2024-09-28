using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LoanManagementSystem.Models;
using Quartz;

namespace LoanManagementSystem.Jobs
{
    public class PaymentTrackingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }

        
    }
}