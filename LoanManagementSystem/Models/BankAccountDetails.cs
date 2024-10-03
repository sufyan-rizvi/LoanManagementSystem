using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class BankAccountDetails
    {
        public virtual Guid AccountDetailId { get; set; }


        [Required(ErrorMessage = "Card Number is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card Number must be exactly 16 digits.")]
        [Display(Name = "Card Number")]
        public virtual string CardNumber { get; set; }

        [Required(ErrorMessage = "Name on Card is required.")]
        [StringLength(100, ErrorMessage = "Name on Card cannot exceed 100 characters.")]
        [Display(Name = "Name on Card")]
        public virtual string NameOnCard { get; set; }

        [Required(ErrorMessage = "Expiry Month is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Expiry Month must be between 01 and 12.")]
        [Display(Name = "Expiry Month")]
        public virtual string ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Expiry Year is required.")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Expiry Year must be exactly 2 digits.")]
        [Display(Name = "Expiry Year")]
        public virtual string ExpiryYear { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be exactly 3 digits.")]
        [Display(Name = "CVV")]
        public virtual string CVV { get; set; }

        public virtual Customer Customer { get; set; }
    }
}