﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanOfficer
    {
        public virtual Guid OfficerId { get; set; }
        public virtual IList<LoanApplication> LoanApplications { get; set; }
        public virtual User User { get; set; }
    }
}