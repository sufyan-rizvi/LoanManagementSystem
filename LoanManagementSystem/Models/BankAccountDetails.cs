using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class BankAccountDetails
    {
        public virtual Guid AccountDetailId {  get; set; }
        [Required]
        [MaxLength(16)]
        [Display(Name ="Card Number")]
        public virtual int CardNumber {  get; set; }
        [Required]
        [Display(Name ="Card Name")]
        public virtual string NameOnCard {  get; set; }
        [Required]
        [MaxLength(2)]
        [Display(Name ="Expiry Month")]
        public virtual string ExpiryMonth {  get; set; }
        [Required]
        [MaxLength(2)]
        [Display(Name ="Expiry Year")]
        public virtual string ExpiryYear {  get; set; }
        [Required]
        [MaxLength(3)]
        public virtual string CVV {  get; set; }
        public virtual Customer Customer { get; set; }

    }
}