using System;
using System.Collections.Generic;
using System.Linq;
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

        public IList<LoanScheme> GetAllSchemes()
        {
            return _customerRepo.GetAllSchemes();
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
    }
}