using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

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
                var pendingLoans = session.Query<LoanApplication>().Where(l => l.Status == ApplicationStatus.PendingApproval).ToList();
                return View(pendingLoans);
            }
        }

        // Action method for displaying pending collateral approvals
        public ActionResult CollateralApproval()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var pendingCollaterals = session.Query<Collateral>().Where(c => c.Status == "Pending").ToList();
                return View(pendingCollaterals);
            }
        }
    }

}