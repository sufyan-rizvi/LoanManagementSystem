using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Repayment
    {

        [Display(Name = "Repayment ID")]
        public virtual Guid RepaymentId { get; set; }

        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)] // This ensures a date picker is shown in the UI
        public virtual DateTime PaymentDate { get; set; }

        [Display(Name = "Amount")]
        [DataType(DataType.Currency)] // This formats the amount as currency in the UI
        public virtual double Amount { get; set; }

        [Display(Name = "Transaction ID")]
        public virtual string TransactionId { get; set; }

        [Display(Name = "Approved")]
        public virtual bool IsApproved { get; set; }

        [Display(Name = "Loan Application")]
        public virtual LoanApplication Application { get; set; }


    }
}