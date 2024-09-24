using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Employee
    {
        public virtual Guid EmpId { get; set; }        
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Role Role { get; set; }
        public virtual Address Address { get; set; }
        public virtual IList<LoanApplication> LoanApplications { get; set; }
    }
}