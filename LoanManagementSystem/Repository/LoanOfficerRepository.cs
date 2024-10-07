using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var documents = _session.Query<RegistrationDocuments>().Where(l => l.Application.ApplicationId == id).ToList();

            return documents;
        }
        public void RegApproveLoan(Guid id)
        {

            var loan = _session.Get<LoanApplication>(id);
            if (loan == null)
            {
                return;
            }
            if (loan.Scheme.SchemeType == SchemeType.Retail)
            {
                loan.Status = ApplicationStatus.LoanRepayment;
                loan.PaymentStartDate = DateTime.Now.AddMonths(1);
                loan.NextPaymentDate = DateTime.Now.AddMonths(1);

            }
            else
            {
                loan.Status = ApplicationStatus.AddCollateral;
            }


            using (var txn = _session.BeginTransaction())
            {
                _session.Update(loan);
                txn.Commit();
            }



        }

        public void RejectApproveLoan(Guid id, string comments)
        {

            var loan = _session.Get<LoanApplication>(id);
            if (loan == null)
            {
                return;
            }

            loan.Comments = comments;
            loan.Status = ApplicationStatus.Rejected;

            using (var txn = _session.BeginTransaction())
            {
                _session.Update(loan);
                txn.Commit();
            }



        }
        public List<LoanApplication> GetCollateralDocuments()
        {

            var pendingCollaterals = _session.Query<LoanApplication>().Fetch(l => l.Applicant).ThenFetch(l => l.User).Where(c => c.Status ==
            ApplicationStatus.CollateralPending).ToList();
            return pendingCollaterals;

        }
        public List<CollateralDocuments> GetToShowCollateralDocuments(Guid id)
        {
            var documents = _session.Query<CollateralDocuments>().Where(r => r.Application.ApplicationId == id).ToList();
            return documents;

        }
        public void ApproveCollateralDocuments(Guid id, string comments)
        {

            var collateral = _session.Get<LoanApplication>(id);
            if (collateral == null)
            {
                return;
            }
            collateral.Comments = comments; 
            collateral.Status = ApplicationStatus.LoanRepayment;
            collateral.PaymentStartDate = DateTime.Now.AddMonths(1);
            collateral.NextPaymentDate = DateTime.Now.AddMonths(1);

            using (var txn = _session.BeginTransaction())
            {
                _session.Update(collateral);
                txn.Commit();
            }



        }
        public void RejectCollateralDocuments(Guid id, string comments)
        {

            var collateral = _session.Get<LoanApplication>(id);
            if (collateral == null)
            {
                return;
            }
            collateral.Comments = comments;
            collateral.Status = ApplicationStatus.Rejected;

            using (var txn = _session.BeginTransaction())
            {
                _session.Update(collateral);
                txn.Commit();
            }



        }



    }
}