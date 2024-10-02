using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.DTOs
{
    public class LoanApplicationMonthWise
    {
        public virtual int Month { get; set; }   // Month of the loan application (1-12)
        public virtual int Year { get; set; }    // Year of the loan application
        public virtual Int64 ApplicationCount { get; set; }  // Number of applications for that month
        public virtual double TotalLoanAmount { get; set; }
    }
}