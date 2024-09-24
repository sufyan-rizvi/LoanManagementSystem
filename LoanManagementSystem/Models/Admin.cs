using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace LoanManagementSystem.Models
{
    public class Admin
    {
        public virtual Guid AdminId { get; set; }
        public virtual User User { get; set; }
        public virtual IList<LoanScheme> Schemes { get; set; }
        public virtual IList<LoanOfficer> LoanOfficer { get; set; }
    }
}