using System;
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
        public virtual double Tenture { get; set; }
        public virtual string Description { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual double EMIAmount
        {
            get
            {
                return (PrincipalAmount * (InterestRate) * Math.Pow(1 + (InterestRate), Tenture)) / (Math.Pow(1 + InterestRate, Tenture - 1));
            }
        }

    }
}