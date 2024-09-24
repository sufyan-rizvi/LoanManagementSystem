using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanApplication
    {
        public virtual Guid ApplicationId { get; set; }
        public virtual DateTime ApplicationDate { get; set; }
        public virtual ApplicationStatus Status { get; set; }
        public virtual Customer Applicant { get; set; }
        public virtual LoanOfficer AssignedOfficer { get; set; }
        public virtual LoanScheme Scheme { get; set; }

    }
}