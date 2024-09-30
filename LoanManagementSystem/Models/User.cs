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
        [Required]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public virtual string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public virtual string PhoneNumber { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Address Address { get; set; }
        public virtual Role Role { get; set; }           
        public virtual IList<Admin> Admins { get; set; }
        public virtual IList<LoanOfficer> LoanOfficers { get; set; }
        public virtual IList<Customer> Customers { get; set; }
        



    }
}