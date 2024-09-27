using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Collateral
    {
        public virtual Guid CollateralId { get; set; }
        public virtual int LoanId { get; set; }
        public virtual decimal CollateralValue { get; set; }
        public virtual string Status { get; set; } // "Pending", "Approved", "Rejected"
    }

}