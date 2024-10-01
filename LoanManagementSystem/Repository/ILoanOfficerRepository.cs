using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    internal interface ILoanOfficerRepository
    {
        List<LoanApplication> GetAllDocuments();
        List<RegistrationDocuments> GetAppDocuments(Guid id);
        void RegApproveLoan(Guid id);
        void RejectApproveLoan(Guid id);
        List<LoanApplication> GetCollateralDocuments();
        List<CollateralDocuments> GetToShowCollateralDocuments(Guid id);
        void ApproveCollateralDocuments(Guid id);
        void RejectCollateralDocuments(Guid id);
    }
}
