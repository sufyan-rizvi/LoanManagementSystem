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
        public virtual string Category { get; set; } // Retail or Corporate
        public virtual double InterestRate { get; set; }
    }
}