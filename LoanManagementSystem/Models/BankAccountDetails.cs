using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class BankAccountDetails
    {
        public virtual Guid AccountDetailId {  get; set; }
        public virtual int CardNumber {  get; set; }
        public virtual string NameOnCard {  get; set; }
        public virtual string ExpiryMonth {  get; set; }
        public virtual string ExpiryYear {  get; set; }
        public virtual string CVV {  get; set; }
        public virtual Customer Customer { get; set; }

    }
}