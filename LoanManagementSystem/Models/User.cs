using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class User
    {
        public virtual Guid UserId { get; set; }
        [Required]
        [Display(Name ="Username")]
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        [Display(Name ="First Name")]
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Address Address { get; set; }
        public virtual Role Role { get; set; }           
        public virtual IList<Admin> Admins { get; set; }
        public virtual IList<LoanOfficer> LoanOfficers { get; set; }
        public virtual IList<Customer> Customers { get; set; }
        



    }
}