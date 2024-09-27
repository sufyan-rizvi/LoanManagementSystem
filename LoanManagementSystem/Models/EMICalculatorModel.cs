using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class EMICalculatorModel
    {
        public decimal LoanAmount { get; set; }       // Principal loan amount
        public decimal InterestRate { get; set; }     // Annual interest rate in percentage
        public int LoanTenure { get; set; }           // Loan tenure in months

        public decimal EMIAmount { get; set; }        // Calculated EMI
    }

}