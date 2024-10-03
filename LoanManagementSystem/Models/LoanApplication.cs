using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanApplication
    {
        public virtual Guid ApplicationId { get; set; }

        [Required(ErrorMessage = "Application Date is required.")]
        [Display(Name = "Application Date")]
        public virtual DateTime ApplicationDate { get; set; }

        [Required(ErrorMessage = "Payment Start Date is required.")]
        [Display(Name = "Payment Start Date")]
        public virtual DateTime PaymentStartDate { get; set; }

        [Required(ErrorMessage = "Next Payment Date is required.")]
        [Display(Name = "Next Payment Date")]
        public virtual DateTime NextPaymentDate { get; set; }

        [Required(ErrorMessage = "Loan Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Loan Amount must be greater than zero.")]
        [Display(Name = "Loan Amount")]
        public virtual double LoanAmount { get; set; }

        [Required(ErrorMessage = "Tenure (months) is required.")]
        [Range(1, 360, ErrorMessage = "Tenure must be between 1 and 360 months.")]
        [Display(Name = "Tenure (months)")]
        public virtual int Tenure { get; set; } // monthly  

        [Display(Name = "Payments Missed")]
        public virtual int PaymentsMissed { get; set; } = 0;

        public virtual int PaymentsMade { get; set; } = 0;

        [Required(ErrorMessage = "EMI Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "EMI Amount must be greater than zero.")]
        [Display(Name = "EMI Amount")]
        public virtual double EMIAmount { get; set; }

        public virtual ApplicationStatus Status { get; set; }

        [Required(ErrorMessage = "Applicant details are required.")]
        [Display(Name = "Applicant")]
        public virtual Customer Applicant { get; set; }

        [Display(Name = "Assigned Officer")]
        public virtual LoanOfficer AssignedOfficer { get; set; }

        [Display(Name = "Loan Scheme")]
        public virtual LoanScheme Scheme { get; set; }

        [Display(Name = "Repayments")]
        public virtual IList<Repayment> Repayments { get; set; } = new List<Repayment>();

        [Display(Name = "Registration Documents")]
        public virtual IList<RegistrationDocuments> RegistrationDocuments { get; set; } = new List<RegistrationDocuments>();

        [Display(Name = "Collateral Documents")]
        public virtual IList<CollateralDocuments> CollateralDocuments { get; set; } = new List<CollateralDocuments>();
    }
}