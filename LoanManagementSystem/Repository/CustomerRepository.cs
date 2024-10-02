using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;
using NHibernate;
using NHibernate.Linq;

namespace LoanManagementSystem.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly ISession _session;

        public CustomerRepository(ISession session)
        {
            _session = session;
        }        

        public IList<LoanScheme> GetAllSchemes()
        {
            return _session.Query<LoanScheme>().Where(l=>l.IsActive).ToList();
        }

        public LoanScheme GetById(Guid id)
        {
            return _session.Query<LoanScheme>().FirstOrDefault(l=>l.LoanSchemeId == id && l.IsActive);
        }

        public IList<LoanApplication> GetAllApplications(Guid id)
        {
            return _session.Query<LoanApplication>().Fetch(l=>l.Applicant).Where(l=>l.Applicant.CustId == id).ToList();
        }

        public void AddloanDetail(LoanApplication application)
        {
            using(var txn =  _session.BeginTransaction())
            {
                _session.Merge(application);
                txn.Commit();   
            }
        }

        public LoanApplication GetApplicationById(Guid id)
        {
            return _session.Query<LoanApplication>().FirstOrDefault(l => l.ApplicationId == id);
        }

        public IList<LoanApplication> GetAllApplications()
        {
            return _session.Query<LoanApplication>().FetchMany(l=>l.Repayments).ToList();
        }

    }
}