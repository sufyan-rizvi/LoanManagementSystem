using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ICustomerService
    {
        IList<LoanScheme> GetAllSchemes();
        LoanScheme GetSchemeById(Guid id);
        IList<LoanApplication> CustomerApplications(Guid id);
        void AddLoanApplication(LoanApplication application);
        LoanApplication GetApplicationById(Guid id);
        IList<LoanApplication> GetAllLoanApplications();

    }
}
