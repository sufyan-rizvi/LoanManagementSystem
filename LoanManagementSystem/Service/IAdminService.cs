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
        LoanOfficer ToggleOfficerDelete(Guid id);
        void EditOfficer(LoanOfficer officer);

        IList<LoanScheme> GetAllSchemes();
        void AddScheme(LoanScheme scheme);
        void EditScheme(Guid id);
        LoanScheme DeleteScheme(Guid id);

    }
}
