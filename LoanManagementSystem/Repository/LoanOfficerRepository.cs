using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using AutoMapper;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using NHibernate;
using NHibernate.Linq;

namespace LoanManagementSystem.Repository
{
    public class LoanOfficerRepository : ILoanOfficerRepository
    {
        private static ISession _session;

        public LoanOfficerRepository(ISession session)
        {
            _session = session;   
        }

        public List<LoanApplication> GetAllDocuments()
        {
            var pendingLoans = _session.Query<LoanApplication>().Fetch(l => l.Applicant).ThenFetch(a => a.User).Where(l => l.Status ==
                ApplicationStatus.PendingApproval).ToList();

                var dto = Mapper.Map<List<LoanApplication>>(pendingLoans);
                return dto;

            
        }
        public List<RegistrationDocuments> GetAppDocuments(Guid id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var loanApplication = session.Get<LoanApplication>(id);
                var documents = session.Query<RegistrationDocuments>().Where(r => r.Customer.CustId ==
                loanApplication.Applicant.CustId).ToList();
                return documents;
            }
        }
        public void RegApproveLoan(Guid id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var loan = session.Get<LoanApplication>(id);
                if (loan == null)
                {
                    return ;
                }
                if (loan.Scheme.SchemeType == SchemeType.Retail)
                {
                    loan.Status = ApplicationStatus.LoanRepayment;
                    loan.PaymentStartDate = DateTime.Now;
                }
                else
                {
                    loan.Status = ApplicationStatus.AddCollateral;
                }


                using (var txn = session.BeginTransaction())
                {
                    session.Update(loan);
                    txn.Commit();
                }

                
            }
        }
        public void RejectApproveLoan(Guid id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var loan = session.Get<LoanApplication>(id);
                if (loan == null)
                {
                    return ;
                }

                loan.Status = ApplicationStatus.Rejected;

                using (var txn = session.BeginTransaction())
                {
                    session.Update(loan);
                    txn.Commit();
                }

                
            }
        }
        public List<LoanApplication> GetCollateralDocuments()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingCollaterals = session.Query<LoanApplication>().Where(c => c.Status ==
                ApplicationStatus.CollateralPending).ToList();
                return pendingCollaterals;
            }
        }
        public List<CollateralDocuments> GetToShowCollateralDocuments(Guid id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var loanApplication = session.Get<LoanApplication>(id);
                var documents = session.Query<CollateralDocuments>().Where(r => r.Customer.CustId ==
                loanApplication.Applicant.CustId).ToList();
                return documents;
            }
        }
        public void ApproveCollateralDocuments(Guid id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var collateral = session.Get<LoanApplication>(id);
                if (collateral == null)
                {
                    return ;
                }

                collateral.Status = ApplicationStatus.LoanRepayment;
                collateral.PaymentStartDate = DateTime.Now;

                using (var txn = session.BeginTransaction())
                {
                    session.Update(collateral);
                    txn.Commit();
                }

                
            }
        }
        public void RejectCollateralDocuments(Guid id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var collateral = session.Get<LoanApplication>(id);
                if (collateral == null)
                {
                    return ;
                }

                collateral.Status = ApplicationStatus.Rejected;

                using (var txn = session.BeginTransaction())
                {
                    session.Update(collateral);
                    txn.Commit();
                }

                
            }
        }



    }
}