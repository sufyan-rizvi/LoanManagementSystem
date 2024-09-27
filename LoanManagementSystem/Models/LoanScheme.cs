﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanScheme
    {
        public virtual Guid LoanSchemeId { get; set; }
        public virtual SchemeType SchemeType { get; set; }
        public virtual double InterestRate { get; set; }
        public virtual double PrincipalAmount { get; set; }
        public virtual double Tenure { get; set; }
        public virtual string LongDescription { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual bool IsActive { get; set; }  
        public virtual IList<Customer> Customers { get; set; }
        public virtual double EMIAmount
        {
            get
            {
                return (PrincipalAmount * (InterestRate) * Math.Pow(1 + (InterestRate), Tenure)) / (Math.Pow(1 + InterestRate, Tenure - 1));
            }
        }

    }
}