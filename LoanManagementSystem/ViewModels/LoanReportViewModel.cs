using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.ViewModels
{
    public class LoanReportViewModel
    {
        public List<Loan> Loans { get; set; }
    }

    public class Loan
    {
        public int LoanId { get; set; }
        public string CustomerName { get; set; }
        public double LoanAmount { get; set; }
        public double InterestRate { get; set; }
        public string Status { get; set; }
    }
}