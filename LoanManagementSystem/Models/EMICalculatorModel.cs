using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class EMICalculatorModel
    {
        [Required]
        [Display(Name ="Loan Amount")]
        public decimal LoanAmount { get; set; }// Principal loan amount
        [Required]
        [Display(Name ="Interest Rate")]
        public decimal InterestRate { get; set; }// Annual interest rate in percentage
        [Required]
        [Display(Name = "Loan Tenure")]
        public int LoanTenure { get; set; }           // Loan tenure in months

        public decimal EMIAmount { get; set; }        // Calculated EMI
    }

}