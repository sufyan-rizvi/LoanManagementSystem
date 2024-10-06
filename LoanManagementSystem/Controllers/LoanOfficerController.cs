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
        private readonly ILoanOfficerService _loanOfficerService;
        private readonly ICustomerService _customerService;
        public LoanOfficerController(ILoanOfficerService loanOfficerService, ICustomerService customerService)
        {
            _loanOfficerService = loanOfficerService;
            _customerService = customerService;
        }

        // Action method for displaying the welcome page
        public ActionResult Welcome()
        {
            return View(_loanOfficerService.GetDocuments());
        }


        // Action method for displaying pending loan approvals
        public ActionResult LoanApproval()
        {
            return View(_loanOfficerService.GetDocuments());

        }

        //Show Rejistration Documents to the Loan Officesr
        public ActionResult GetLoanDocuments(Guid id)
        {
            Session["registerApplicationId"] = id;
            var application = _customerService.GetApplicationById(id);
            return View(application);

        }
        public ActionResult ApproveLoan(Guid id)
        {

            _loanOfficerService.RegApproveLoan((Guid)id);
            return RedirectToAction("LoanApproval");

        }

        // Action method for rejecting a loan
        public ActionResult RejectLoan(Guid id, string comments)
        {
            //var id = TempData.Peek("registerApplicationId");
            _loanOfficerService.RejectApproveLoan((Guid)id, comments);
            return RedirectToAction("LoanApproval");

        }


        // Action method for displaying pending collateral approvals
        public ActionResult CollateralApproval()
        {
            return View(_loanOfficerService.GetCollateralDocuments());

        }
        //Show Collateral Documents to the Loan Officesr
        public ActionResult GetCollateralDocuments(Guid id)
        {
            TempData["collateralApplicationId"] = id;

            var application = _customerService.GetApplicationById(id);
            return View(application);


        }
        // Action method for approving a collateral
        public ActionResult ApproveCollateral(Guid id, string comments)
        {
            //var id = TempData.Peek("collateralApplicationId");
            _loanOfficerService.ApproveCollateralDocuments((Guid)id, comments);
            return RedirectToAction("CollateralApproval");

        }

        // Action method for rejecting a collateral
        public ActionResult RejectCollateral(Guid id, string comments)
        {
            
            _loanOfficerService.RejectCollateralDocuments((Guid)id, comments);
            return RedirectToAction("CollateralApproval");

        }

    }

}