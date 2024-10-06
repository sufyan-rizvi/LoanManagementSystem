using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace LoanManagementSystem.Service
{
    public class CustomerService:ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public IList<LoanApplication> ApplicationsWithEmailDue()
        {
            return _customerRepo.ApplicationsWithEmailDue();
        }

        public void SetApplicationsToFalse(IList<LoanApplication> applications)
        {
            _customerRepo.SetApplicationsToFalse(applications);
        }

        public IList<LoanApplication> ApplicationsToBeMadeFalse()
        {
            return _customerRepo.ApplicationsToBeMadeFalse();
        }

        public IList<LoanApplication> GetNPAApplications()
        {
            return _customerRepo.GetNPAApplications();
        }
        public void CheckNPA(IList<LoanApplication> applications)
        {
            _customerRepo.CheckNPA(applications);
        }

        public IList<LoanApplication> PaymentMissedApplications()
        {
            return _customerRepo.PaymentMissedApplications();
        }

        public void UpdateNextPaymentDate(IList<LoanApplication> applications)
        {
            _customerRepo.UpdateNextPaymentDate(applications);
        }


        public IList<LoanScheme> GetAllSchemes()
        {
            return _customerRepo.GetAllSchemes();
        }

        public IList<Repayment> GetAllPaymentsForApplication(Guid id)
        {
            return _customerRepo.GetAllPaymentsForApplication(id);
        }

        public LoanScheme GetSchemeById(Guid id)
        {
            return _customerRepo.GetById(id);
        }

        public IList<LoanApplication> CustomerApplications(Guid id)
        {
            return _customerRepo.GetAllApplications(id);
        }

        public void AddLoanApplication(LoanApplication application)
        {
            _customerRepo.AddloanDetail(application);
        }

        public LoanApplication GetApplicationById(Guid id)
        {
           return  _customerRepo.GetApplicationById(id);   
        }

        public IList<LoanApplication> GetAllLoanApplications()
        {
            return _customerRepo.GetAllApplications();
        }
    }
}