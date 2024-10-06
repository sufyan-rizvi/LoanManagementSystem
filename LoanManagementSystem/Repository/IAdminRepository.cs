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
        LoanOfficer GetByOfficerEmail(string email);
        LoanOfficer GetByOfficerUsername(string username);
        List<LoanApplication> LoanReport();
        List<LoanApplication> NPAReport();
        List<LoanScheme> SchemesReportk();
        List<LoanOfficer> LoanOfficerReport();
        void AddOfficer(LoanOfficer officer);
        void UpdateOfficer(LoanOfficer officer);
        void DeleteOfficer(LoanOfficer officer);
        IList<LoanOfficer> GetAllActiveOfficers();
        IList<LoanOfficer> GetAllOfficers();
        LoanScheme GetSchemeById(Guid id);
        IList<LoanScheme> GetAllSchemes();
        void AddScheme(LoanScheme scheme);
        void UpdateScheme(LoanScheme scheme);
        void DeleteScheme(LoanScheme scheme);

    }
}
