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

        [Required(ErrorMessage = "Aadhar Number is required.")]
        [Display(Name = "Aadhar Number")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar Number must be exactly 12 digits.")]
        public virtual string AadharNumber { get; set; }

        [Required(ErrorMessage = "PAN Number is required.")]
        [Display(Name = "PAN Number")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "PAN Number must be in the format AAAAA9999A.")]
        public virtual string PANNumber { get; set; }

        public virtual User User { get; set; }

        [Display(Name = "Loan Applications")]
        public virtual IList<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();

        [Display(Name = "Bank Account Details")]
        public virtual BankAccountDetails BankAccountDetails { get; set; }

        [Display(Name = "Repayments")]
        public virtual IList<Repayment> Repayments { get; set; } = new List<Repayment>();
    }
}