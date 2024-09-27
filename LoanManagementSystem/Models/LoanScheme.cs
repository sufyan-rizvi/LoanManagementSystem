using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanScheme
    {
        //public virtual Guid LoanSchemeId { get; set; }
        //public virtual SchemeType SchemeType { get; set; }
        //public virtual double InterestRate { get; set; }
        //public virtual Admin Admin { get; set; }

        public virtual Guid SchemeId { get; set; }
        public virtual string SchemeName { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string FullDescription { get; set; }
        public virtual string Category { get; set; } // "Retail" or "Corporate"
        public virtual decimal InterestRate { get; set; }
        public virtual int LoanTenure { get; set; } // In years
        public virtual string EligibilityCriteria { get; set; }
        public virtual Admin Admin { get; set; }
    }
}