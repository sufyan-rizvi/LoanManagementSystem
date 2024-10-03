using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public enum ApplicationStatus
    {
        [Display(Name = "Pending Approval")]
        PendingApproval,

        [Display(Name = "Approved")]
        Approved,

        [Display(Name = "Add Collateral")]
        AddCollateral,

        [Display(Name = "Collateral Approval Pending")]
        CollateralPending,

        [Display(Name = "Collateral Approved")]
        CollateralApproved,

        [Display(Name = "Active")]
        LoanRepayment,

        [Display(Name = "Loan Closed")]
        LoanClosed,

        [Display(Name = "Rejected")]
        Rejected,

        [Display(Name = "Contact Bank")]
        NPA

    }
}