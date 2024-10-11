using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IAdminService
    {
        IList<LoanOfficer> GetAllOfficers();
        List<LoanApplication> LoanReport();
        IList<LoanApplicationMonthWise> AmountResult();
        IList<LoanApplicationMonthWise> NPAResult();
        IList<LoanApplicationMonthWise> LoanApplicationMonthWise();
        List<LoanApplication> NPAReport();
        List<LoanScheme> SchemesReportk();
        List<LoanOfficer> LoanOfficerReport();
        void AddOfficer(LoanOfficer officer);
        LoanOfficer ToggleOfficerDelete(Guid id);
        void EditOfficer(LoanOfficer officer);
        LoanOfficer GetOfficerById(Guid id);
        IList<LoanOfficer> GetAllActiveOfficers();
        IList<LoanScheme> GetAllSchemes();
        void AddScheme(LoanScheme scheme);
        LoanScheme GetSchemeById(Guid id);
        void EditScheme(LoanScheme scheme);
        LoanScheme DeleteScheme(Guid id);

    }
}
