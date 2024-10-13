using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;
using static System.Net.Mime.MediaTypeNames;

namespace LoanManagementSystem.Service
{
    public interface ICustomerService
    {
        IList<LoanApplication> PaymentMissedApplications();
        IList<LoanApplication> ApplicationsWithEmailDue();
        void SetApplicationsToFalse(IList<LoanApplication> applications);
        IList<LoanApplication> ApplicationsToBeMadeFalse();
        void CheckNPA(IList<LoanApplication> applications);
        IList<LoanApplication> GetNPAApplications();
        void UpdateNextPaymentDate(IList<LoanApplication> applications);
        void UpdateApplication(LoanApplication application);
        IList<LoanScheme> GetAllSchemes();
        LoanScheme GetSchemeById(Guid id);
        IList<LoanApplication> CustomerApplications(Guid id);
        void AddLoanApplication(LoanApplication application);
        LoanApplication GetApplicationById(Guid id);
        IList<LoanApplication> GetAllLoanApplications();
        IList<Repayment> GetAllPaymentsForApplication(Guid id);

    }
}
