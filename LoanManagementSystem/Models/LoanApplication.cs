using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanApplication
    {
        public virtual int ApplicationId { get; set; }
        public virtual User Applicant { get; set; }
        public virtual Employee AssignedOfficer { get; set; }
        public virtual LoanScheme Scheme { get; set; }
        public virtual DateTime ApplicationDate { get; set; }
        public virtual ApplicationStatus Status { get; set; }

    }
}