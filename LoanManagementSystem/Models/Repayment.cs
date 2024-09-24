﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Repayment
    {
        public virtual int RepaymentId { get; set; }
        public virtual LoanApplication LoanApplication { get; set; }
        public virtual DateTime PaymentDate { get; set; }
        public virtual double Amount { get; set; }
        public virtual bool IsCompleted { get; set; }

    }
}