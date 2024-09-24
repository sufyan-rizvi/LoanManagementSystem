using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace LoanManagementSystem.Models
{
    public class LoanOfficer
    {
        public virtual Guid OfficerId { get; set; }
        public virtual IList<LoanApplication> LoanApplications { get; set; }
        public virtual User User { get; set; }
        public virtual Admin Admin { get; set; }
    }
}