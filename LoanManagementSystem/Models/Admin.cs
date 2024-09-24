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
        public User User { get; set; }
        public virtual List<LoanOfficer> LoanOfficer { get; set; }
    }
}