using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Customer
    {
        public virtual Guid CustId { get; set; }
        [Required]
        [Display(Name ="Aadhar Number")]
        public virtual string AadharNumber { get; set; }
        [Required]
        [Display(Name ="PAN Number")]
        public virtual string PANNumber { get; set; }
        public virtual int PaymentsMissed { get; set; } = 0;
        public virtual User User { get; set; }
        public virtual IList<LoanApplication> LoanApplications { get; set; }
        public virtual BankAccountDetails BankAccountDetails { get; set; }
        public virtual IList<RegistrationDocuments> RegistrationDocuments { get; set; }
        public virtual IList<CollateralDocuments> CollateralDocuments { get; set; }
        public virtual IList<Repayment> Repayments { get; set; }
        

    }
}