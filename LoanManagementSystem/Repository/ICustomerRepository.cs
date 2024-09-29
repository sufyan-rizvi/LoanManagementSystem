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
        IList<LoanScheme> GetAllSchemes();
        LoanScheme GetById(Guid id);
        IList<LoanApplication> GetAllApplications(Guid id);
        void AddloanDetail(LoanApplication application);
    }
}
