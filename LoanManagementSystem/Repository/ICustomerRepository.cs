using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface ICustomerRepository
    {
        IList<LoanApplication> PaymentMissedApplications();
        IList<LoanApplication> ApplicationsWithEmailDue();
        void UpdateApplication(LoanApplication application);
        void SetApplicationsToFalse(IList<LoanApplication> applications);
        void CheckNPA(IList<LoanApplication> applications);
        IList<LoanApplication> ApplicationsToBeMadeFalse();
        IList<LoanApplication> GetNPAApplications();
        void UpdateNextPaymentDate(IList<LoanApplication> applications);
        IList<LoanScheme> GetAllSchemes();
        LoanScheme GetById(Guid id);
        IList<LoanApplication> GetAllApplications(Guid id);
        void AddloanDetail(LoanApplication application);
        LoanApplication GetApplicationById(Guid id);
        IList<LoanApplication> GetAllApplications();
        IList<Repayment> GetAllPaymentsForApplication(Guid id);
    }
}
