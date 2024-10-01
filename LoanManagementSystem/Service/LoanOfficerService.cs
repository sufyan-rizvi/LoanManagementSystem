using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public class LoanOfficerService: ILoanOfficerService
    {
        private static ILoanOfficerRepository _loanOfficerRepository;
        public LoanOfficerService(LoanOfficerRepository loanOfficerRepository)
        {
            _loanOfficerRepository = loanOfficerRepository;
        }

        public  List<LoanApplication> GetDocuments()
        {
            return _loanOfficerRepository.GetAllDocuments();
        }
        public List <RegistrationDocuments> GetAppDocuments(Guid id)
        {
            return _loanOfficerRepository.GetAppDocuments(id);
        }
        public void RegApproveLoan(Guid id)
        {
            _loanOfficerRepository.RegApproveLoan(id);
        }
        public void RejectApproveLoan(Guid id)
        {
            _loanOfficerRepository.RejectApproveLoan(id);
        }
        public List<LoanApplication> GetCollateralDocuments()
        {
            return _loanOfficerRepository.GetCollateralDocuments();
        }
        public List<CollateralDocuments> GetToShowCollateralDocuments(Guid id)
        {
            return _loanOfficerRepository.GetToShowCollateralDocuments(id);
        }
        public void ApproveCollateralDocuments(Guid id)
        {
            _loanOfficerRepository.ApproveCollateralDocuments(id);
        }
        public void RejectCollateralDocuments(Guid id)
        {
            _loanOfficerRepository.RejectCollateralDocuments(id);
        }
    }
}