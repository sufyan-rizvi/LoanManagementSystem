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
        [Display(Name ="Application Date")]
        public virtual DateTime ApplicationDate { get; set; }
        [Display(Name = "Start Date")]
        public virtual DateTime PaymentStartDate { get; set; }
        [Display(Name = "Next Payment Date")]
        public virtual DateTime NextPaymentDate { get; set; }
        [Display(Name = "Loan Amount")]        
        public virtual double LoanAmount { get; set; }
        [Required]
        [Display(Name = "Tenure (months)")]
        public virtual int Tenure { get; set; }//monthly  
        [Display(Name = "Payments Missed")]
        public virtual int PaymentsMissed { get; set; } = 0;
        public virtual int PaymentsMade { get; set; } = 0;
        [Display(Name = "EMI Amount")]
        public virtual double EMIAmount { get; set; }
        public virtual ApplicationStatus Status { get; set; } 
        public virtual Customer Applicant { get; set; }
        public virtual LoanOfficer AssignedOfficer { get; set; }
        public virtual LoanScheme Scheme { get; set; }
        public virtual IList<Repayment> Repayments { get; set; }
        public virtual IList<RegistrationDocuments> RegistrationDocuments { get; set; }
        public virtual IList<CollateralDocuments> CollateralDocuments { get; set; }
        
    }
}