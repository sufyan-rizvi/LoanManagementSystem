using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using LoanManagementSystem.Models;
using NHibernate;

namespace LoanManagementSystem.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ISession _session;
        public PaymentRepository(ISession session)
        {
            _session = session;

        }
        public void SaveRepayment(Repayment repayment)
        {
            using (var txn = _session.BeginTransaction())
            {
                _session.Save(repayment);
                txn.Commit();
            }

        }
    }
}