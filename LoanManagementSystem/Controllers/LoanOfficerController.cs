using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using NHibernate.Linq;

namespace LoanManagementSystem.Controllers
{
    //[Authorize(Roles = "LoanOfficer")]
    public class LoanOfficerController : Controller
    {
        
        // Action method for displaying the welcome page
        public ActionResult Welcome()
        {
            return View();
        }
        

        // Action method for displaying pending loan approvals
        public ActionResult LoanApproval()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingLoans = session.Query<LoanApplication>().Fetch(l => l.Applicant).ThenFetch(a => a.User).Where(l => l.Status ==
                ApplicationStatus.PendingApproval).ToList();

                var dto = Mapper.Map<List<LoanApplication>>(pendingLoans);
                return View(dto);
            }
        }
        //Show Rejistration Documents to the Loan Officesr
        public ActionResult GetLoanDocuments(Guid id)
        {
            TempData["registerApplicationId"] = id;
            using (var session = NhibernateHelper.CreateSession())
            {
                var loanApplication = session.Get<LoanApplication>(id);
                var documents = session.Query<RegistrationDocuments>().Where(r => r.Customer.CustId == 
                loanApplication.Applicant.CustId).ToList();
                return View(documents);
            }
        }
        public ActionResult ApproveLoan()
        {

            var id = TempData.Peek("registerApplicationId");
            using (var session = NhibernateHelper.CreateSession())
            {
                var loan = session.Get<LoanApplication>(id);
                if (loan == null)
                {
                    return HttpNotFound();
                }
                if (loan.Scheme.SchemeType == SchemeType.Retail)
                {
                    loan.Status = ApplicationStatus.LoanRepayment;
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

                TempData["SuccessMessage"] = $"Loan {loan.ApplicationId} has been approved.";
                return RedirectToAction("LoanApproval");
            }
        }

        // Action method for rejecting a loan
        public ActionResult RejectLoan()
        {
            var id = TempData.Peek("registerApplicationId");
            using (var session = NhibernateHelper.CreateSession())
            {
                var loan = session.Get<LoanApplication>(id);
                if (loan == null)
                {
                    return HttpNotFound();
                }

                loan.Status = ApplicationStatus.Rejected;

                using (var txn = session.BeginTransaction())
                {
                    session.Update(loan);
                    txn.Commit();
                }

                TempData["SuccessMessage"] = $"Loan {loan.ApplicationId} has been rejected.";
                return RedirectToAction("LoanApproval");
            }
        }









        // Action method for displaying pending collateral approvals
        public ActionResult CollateralApproval()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingCollaterals = session.Query<LoanApplication>().Where(c => c.Status == 
                ApplicationStatus.CollateralPending).ToList();
                return View(pendingCollaterals);
            }
        }
        //Show Collateral Documents to the Loan Officesr
        public ActionResult GetCollateralDocuments(Guid id)
        {
            TempData["collateralApplicationId"] = id;
            using (var session = NhibernateHelper.CreateSession())
            {
                var loanApplication = session.Get<LoanApplication>(id);
                var documents = session.Query<CollateralDocuments>().Where(r => r.Customer.CustId == 
                loanApplication.Applicant.CustId).ToList();
                return View(documents);
            }
        }
        // Action method for approving a collateral
        public ActionResult ApproveCollateral()
        {
            var id = TempData.Peek("collateralApplicationId");
            using (var session = NhibernateHelper.CreateSession())
            {
                var collateral = session.Get<LoanApplication>(id);
                if (collateral == null)
                {
                    return HttpNotFound();
                }

                collateral.Status = ApplicationStatus.LoanRepayment;

                using (var txn = session.BeginTransaction())
                {
                    session.Update(collateral);
                    txn.Commit();
                }

                TempData["SuccessMessage"] = $"Collateral {collateral.ApplicationId} for Loan  has been approved.";
                return RedirectToAction("CollateralApproval");
            }
        }

        // Action method for rejecting a collateral
        public ActionResult RejectCollateral()
        {
            var id = TempData.Peek("collateralApplicationId");
            using (var session = NhibernateHelper.CreateSession())
            {
                var collateral = session.Get<LoanApplication>(id);
                if (collateral == null)
                {
                    return HttpNotFound();
                }

                collateral.Status =ApplicationStatus.Rejected;

                using (var txn = session.BeginTransaction())
                {
                    session.Update(collateral);
                    txn.Commit();
                }

                TempData["SuccessMessage"] = $"Collateral {collateral.ApplicationId} for Loan has been rejected.";
                return RedirectToAction("CollateralApproval");
            }
        }
        
    }

}