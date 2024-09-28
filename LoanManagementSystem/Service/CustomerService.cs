using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class CustomerService:ICustomerService
    {
        private readonly ICustomerRepository _schemeRepo;

        public CustomerService(ICustomerRepository schemeRepo)
        {
            _schemeRepo = schemeRepo;
        }

        public IList<LoanScheme> GetAllSchemes()
        {
            return _schemeRepo.GetAllSchemes();
        }

        public LoanScheme GetSchemeById(Guid id)
        {
            return _schemeRepo.GetById(id);
        }

        public IList<LoanApplication> CustomerApplications(Guid id)
        {
            return _schemeRepo.GetAllApplications(id);
        }
    }
}