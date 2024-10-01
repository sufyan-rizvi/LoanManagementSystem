using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.Security;
using System.Web;

namespace LoanManagementSystem.ViewModels
{
    public class LoginVM
    {
        [Required]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string LoginType { get; set; }


    }
}