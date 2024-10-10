using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using LoanManagementSystem.Data;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using BC = BCrypt.Net.BCrypt;

namespace LoanManagementSystem.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ISession _session;

        public AdminRepository(ISession session)
        {
            _session = session;
        }

        public IList<LoanApplicationMonthWise> AmountResult()
        {
            return _session.CreateQuery(@"
        select 
            month(ApplicationDate) as Month, 
            year(ApplicationDate) as Year, 
            count(*) as ApplicationCount, 
            sum(LoanAmount) as TotalLoanAmount
        from LoanApplication 
        where Status = :status
        group by 
            month(ApplicationDate), 
            year(ApplicationDate)
        order by 
            Year, Month")
        .SetParameter("status", ApplicationStatus.NPA)
        .SetResultTransformer(Transformers.AliasToBean<LoanApplicationMonthWise>())
        .List<LoanApplicationMonthWise>();
        }


        public IList<LoanApplicationMonthWise> NPAResult()
        {
            return _session.CreateQuery(@"
        select 
            month(ApplicationDate) as Month, 
            year(ApplicationDate) as Year, 
            count(*) as ApplicationCount 
        from LoanApplication 
        where Status = :status
        group by 
            month(ApplicationDate), 
            year(ApplicationDate)
        order by 
            Year, Month")
        .SetParameter("status", ApplicationStatus.NPA) // Set the status parameter
        .SetResultTransformer(Transformers.AliasToBean<LoanApplicationMonthWise>())
        .List<LoanApplicationMonthWise>();
        }

        public IList<LoanApplicationMonthWise> LoanApplicationMonthWise()
        {
            return _session.CreateQuery(@"
        select 
            month(ApplicationDate) as Month, 
            year(ApplicationDate) as Year, 
            count(*) as ApplicationCount 
        from LoanApplication 
        group by 
            month(ApplicationDate), 
            year(ApplicationDate)
        order by 
            Year, Month")
                    .SetResultTransformer(Transformers.AliasToBean<LoanApplicationMonthWise>())
                    .List<LoanApplicationMonthWise>();
        }

        public void AddOfficer(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                officer.User.Role = new Role();
                officer.User.Role.RoleName = "LoanOfficer";
                officer.User.Role.User = officer.User;
                officer.User.Password = BC.EnhancedHashPassword(officer.User.Password, 6);
                officer.User.Address.User = officer.User;
                officer.User.IsActive = true;
                _session.Save(officer);
                txn.Commit();
            }
        }
        public List<LoanApplication> LoanReport()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingLoans = session.Query<LoanApplication>().Fetch(l => l.Applicant).ThenFetch(a => a.User).ToList();

                var dto = Mapper.Map<List<LoanApplication>>(pendingLoans);
                return dto;
            }
        }
        public List<LoanApplication> NPAReport()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingLoans = session.Query<LoanApplication>().Fetch(l => l.Applicant).ThenFetch(a => a.User).Where(l => l.Status ==
                ApplicationStatus.NPA).ToList();
                var dto = Mapper.Map<List<LoanApplication>>(pendingLoans);
                return dto;
            }
        }
        public List<LoanScheme> SchemesReportk()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingLoans = session.Query<LoanScheme>().ToList();
                var dto = Mapper.Map<List<LoanScheme>>(pendingLoans);
                return dto;
            }
        }
        public List<LoanOfficer> LoanOfficerReport()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingLoans = session.Query<LoanOfficer>().Fetch(l => l.User).ToList();
                var dto = Mapper.Map<List<LoanOfficer>>(pendingLoans);
                return dto;
            }
        }


        public void AddScheme(LoanScheme scheme)
        {
            using (var txn = _session.BeginTransaction())
            {
                scheme.IsActive = true;
                _session.Save(scheme);
                txn.Commit();
            }
        }

        public void DeleteOfficer(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                var existingOfficer = _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == officer.OfficerId);
                existingOfficer.User.IsActive = !existingOfficer.User.IsActive;
                _session.Update(existingOfficer);
                txn.Commit();
            }
        }
        public IList<LoanOfficer> GetAllOfficers()
        {
            return _session.Query<LoanOfficer>().Fetch(l => l.User).ThenFetch(u => u.Address).ToList();
        }

        public IList<LoanOfficer> GetAllActiveOfficers()
        {
            return _session.Query<LoanOfficer>().Fetch(l => l.User).ThenFetch(u => u.Address).Where(l => l.User.IsActive).ToList();
        }

        public void DeleteScheme(LoanScheme scheme)
        {
            using (var txn = _session.BeginTransaction())
            {
                var existingScheme = _session.Query<LoanScheme>().FirstOrDefault(s => s.LoanSchemeId == scheme.LoanSchemeId);
                existingScheme.IsActive = !existingScheme.IsActive;
                _session.Update(existingScheme);
                txn.Commit();
            }
        }



        public IList<LoanScheme> GetAllSchemes()
        {
            return _session.Query<LoanScheme>().ToList();
        }

        public LoanOfficer GetByOfficerEmail(string email)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.User.Email == email);
        }

        public LoanOfficer GetByOfficerId(Guid id)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == id);
        }

        public LoanOfficer GetByOfficerUsername(string username)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.User.Username == username);
        }

        public void UpdateOfficer(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                var existingOfficer = _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == officer.OfficerId);
                if (existingOfficer != null)
                {
                    existingOfficer.User = officer.User;

                    _session.Merge(existingOfficer);
                    txn.Commit();
                }
            }
        }

        public void UpdateScheme(LoanScheme scheme)
        {
            using (var txn = _session.BeginTransaction())
            {
                scheme.IsActive = true;
                _session.Merge(scheme);
                txn.Commit();
            }
        }

        public LoanScheme GetSchemeById(Guid id)
        {
            return _session.Query<LoanScheme>().FirstOrDefault(s => s.LoanSchemeId == id);
        }
    }
}