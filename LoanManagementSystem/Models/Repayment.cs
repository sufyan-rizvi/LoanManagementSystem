using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Repayment
    {
        public virtual Guid RepaymentId { get; set; }
        public virtual DateTime StartDate { get; set; }
        private DateTime _endDate;
        public virtual DateTime EndDate
        {
            get
            {
                // Calculate EndDate if it hasn't been set
                return _endDate == default ? StartDate.AddMonths(Application.Tenure) : _endDate;
            }
            set
            {
                // Set the backing field
                _endDate = value;
            }
        }
        public virtual double EMIAmount
        {
            get
            {
                return (Application.LoanAmount * (Application.Scheme.InterestRate) * Math.Pow(1 + (Application.Scheme.InterestRate), Application.Tenure)) / (Math.Pow(1 + Application.Scheme.InterestRate, Application.Tenure) - 1);
            }
        }
        //public virtual DateTime NextPaymentDate { get; set; }
        public virtual double TotalAmount { get; set; }
  
        public virtual LoanApplication Application { get; set; }
        //public virtual bool IsCompleted { get; set; }
        //public virtual Customer Customer { get; set; }


    }
}