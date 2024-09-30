using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public enum ApplicationStatus
    {
        PendingApproval, Approved,CollateralPending, CollateralApproved, LoanRepayment, RepaymentCompleted,Rejected
        
    }
}