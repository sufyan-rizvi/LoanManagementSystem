using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;
using NHibernate;
using NHibernate.Linq;

namespace LoanManagementSystem.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly ISession _session;

        public CustomerRepository(ISession session)
        {
            _session = session;
        }

        public void UpdateNextPaymentDate(IList<LoanApplication> applications)
        {
            using (var txn = _session.BeginTransaction())
            {
                foreach (var application in applications)
                {
                    application.PaymentsMissed += 1;
                    if (application.PaymentsMissed >= 3)
                    {
                        application.Status = ApplicationStatus.NPA;
                    }
                    application.NextPaymentDate = application.NextPaymentDate.AddMonths(1);
                    _session.Merge(application);
                }
                txn.Commit();
            }
        }

        public IList<LoanApplication> ApplicationsWithEmailDue()
        {
            try
            {
                var oneDayBeforeNPD = DateTime.Now.AddDays(1);
                var oneDayBeforeExtenstion = DateTime.Now.AddDays(-4);

                var applications = _session.Query<LoanApplication>().Where(l =>
                    l.NextPaymentDate != DateTime.MinValue &&
                    oneDayBeforeNPD >= l.NextPaymentDate &&
                    oneDayBeforeExtenstion <= l.NextPaymentDate &&
                    l.Status == ApplicationStatus.LoanRepayment && !l.EMIPaymentMade
                ).ToList();
                return applications;
            }
            catch (Exception ex)
            {
                return new List<LoanApplication>();
            }
        }

        public void SetApplicationsToFalse(IList<LoanApplication> applications)
        {
            using (var txn = _session.BeginTransaction())
            {
                try
                {
                    foreach (var application in applications)
                    {
                        application.EMIPaymentMade = false;
                        _session.Merge(application);
                    }
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    Console.WriteLine($"Error processing loan applications: {ex.Message}");
                }
            }
        }

        public IList<LoanApplication> ApplicationsToBeMadeFalse()
        {
            try
            {
                var tomorrowIsPayDay = DateTime.Now.AddDays(1).Date;
                _session.Clear();
                var applications = _session.Query<LoanApplication>().Where(l =>

                     l.Status == ApplicationStatus.LoanRepayment &&
                    l.NextPaymentDate != DateTime.MinValue &&
                    tomorrowIsPayDay == l.NextPaymentDate.Date
                ).ToList();
                return applications;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<LoanApplication>();
            }
        }



        public void CheckNPA(IList<LoanApplication> applications)
        {
            using (var txn = _session.BeginTransaction())
            {
                try
                {
                    foreach (var application in applications)
                    {

                        int months = ((DateTime.Now.Year - application.PaymentStartDate.Year) * 12) + DateTime.Now.Month - application.PaymentStartDate.Month;

                        // Calculate missed payments
                        application.PaymentsMissed = Math.Max(0, months - application.PaymentsMade);


                        // Set status to NPA if missed payments are 3 or more
                        if (application.PaymentsMissed >= 3)
                        {
                            application.Status = ApplicationStatus.NPA;
                        }

                        // Merge application with the session
                        _session.Merge(application);


                        // Commit the transaction after processing all applications

                    }
                    txn.Commit();
                }


                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    txn.Rollback();
                    // Log the exception (consider using a logging framework)
                    Console.WriteLine($"Error processing loan applications: {ex.Message}");
                }
            }

        }


        public IList<LoanApplication> GetNPAApplications()
        {
            _session.Clear();
            var dateFiveDaysAfter = DateTime.Now.AddDays(-5);
            return _session.Query<LoanApplication>().Where(l =>
            l.Status == ApplicationStatus.LoanRepayment &&
            dateFiveDaysAfter >= l.NextPaymentDate).ToList();
        }

        public IList<LoanApplication> PaymentMissedApplications()
        {
            _session.Clear();
            var dateSixDaysAfter = DateTime.Now.AddDays(-5).Date;
            var applications = _session.Query<LoanApplication>().AsEnumerable().Where(l =>
                 l.Status == ApplicationStatus.LoanRepayment &&
                dateSixDaysAfter == l.NextPaymentDate.Date &&
                !l.EMIPaymentMade
            ).ToList();
            return applications;
        }

        public IList<LoanScheme> GetAllSchemes()
        {
            return _session.Query<LoanScheme>().Where(l => l.IsActive).ToList();
        }

        public LoanScheme GetById(Guid id)
        {
            return _session.Query<LoanScheme>().FirstOrDefault(l => l.LoanSchemeId == id && l.IsActive);
        }

        public IList<LoanApplication> GetAllApplications(Guid id)
        {
            _session.Clear();
            return _session.Query<LoanApplication>().Fetch(l => l.Applicant).Where(l => l.Applicant.CustId == id).ToList();
        }

        public void AddloanDetail(LoanApplication application)
        {
            using (var txn = _session.BeginTransaction())
            {
                _session.Merge(application);
                txn.Commit();
            }
        }

        public LoanApplication GetApplicationById(Guid id)
        {
            var application = _session.Query<LoanApplication>().Fetch(a=>a.Applicant).ThenFetch(a=>a.User).FirstOrDefault(l => l.ApplicationId == id);
            _session.Evict(application);
            return application;
        }

        public IList<LoanApplication> GetAllApplications()
        {

            var applications = _session.Query<LoanApplication>().FetchMany(l => l.Repayments).ToList();

            return applications;
        }

        public IList<Repayment> GetAllPaymentsForApplication(Guid id)
        {
            return _session.Query<Repayment>().Where(r => r.Application.ApplicationId == id).ToList();
        }

        public void UpdateApplication(LoanApplication application)
        {
            using(var txn = _session.BeginTransaction())
            {
                _session.Merge(application);
                txn.Commit();
            }
        }

    }
}