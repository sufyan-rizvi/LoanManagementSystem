using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Repayment
    {
        public virtual Guid RepaymentId { get; set; }
        public virtual DateTime PaymentDate { get; set; }
        public virtual double Amount { get; set; }
        public virtual string TransactionId { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual LoanApplication Application { get; set; }

    }
}