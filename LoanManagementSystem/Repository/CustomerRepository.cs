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
            return _session.Query<LoanScheme>().ToList();
        }

        public LoanScheme GetById(Guid id)
        {
            return _session.Get<LoanScheme>(id);
        }

        public IList<LoanApplication> GetAllApplications(Guid id)
        {
            return _session.Query<LoanApplication>().Fetch(l=>l.Applicant).Where(l=>l.Applicant.CustId == id).ToList();
        }
    }
}