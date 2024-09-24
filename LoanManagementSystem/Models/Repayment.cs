using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Repayment
    {
        public virtual int RepaymentId { get; set; }
        public virtual LoanApplication LoanApplication { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual DateTime Interval { get; set; }
        public virtual double Amount { get; set; }
        public virtual bool IsCompleted { get; set; }

    }
}