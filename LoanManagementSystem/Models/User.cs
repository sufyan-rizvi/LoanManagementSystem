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

        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        
        public virtual string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email")]
        
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public virtual string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(18, 99, ErrorMessage = "Age must be between 18 and 99.")]
        public virtual int Age { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual Address Address { get; set; }
        public virtual Role Role { get; set; }
        public virtual IList<Admin> Admins { get; set; }
        public virtual IList<LoanOfficer> LoanOfficers { get; set; }
        public virtual IList<Customer> Customers { get; set; }
    }
}