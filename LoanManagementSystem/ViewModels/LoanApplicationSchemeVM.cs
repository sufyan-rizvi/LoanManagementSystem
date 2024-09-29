using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.ViewModels
{
    public class LoanApplicationSchemeVM
    {
        public LoanApplication LoanApplication { get; set; }
        public LoanScheme LoanScheme { get; set; }
        public RegistrationDocuments RegistrationDocuments { get; set; }
    }
}