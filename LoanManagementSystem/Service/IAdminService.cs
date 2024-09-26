using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IAdminService
    {
        IList<LoanOfficer> GetAllOfficers();
        void AddOfficer(LoanOfficer officer);
        LoanOfficer ToggleDelete(Guid id);
        void EditOfficer(LoanOfficer officer);
    }
}
