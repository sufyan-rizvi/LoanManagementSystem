using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IAdminRepository
    {
        LoanOfficer GetByOfficerId(Guid id);
        LoanOfficer GetByEmail(string email);
        LoanOfficer GetByUsername(string username);
        void Add(LoanOfficer officer);
        void Update(LoanOfficer officer);
        void Delete(LoanOfficer officer);
        IList<LoanOfficer> GetAll();
    }
}
