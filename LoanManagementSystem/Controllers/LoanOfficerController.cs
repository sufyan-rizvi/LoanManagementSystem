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
        private readonly LoanOfficerService _loanOfficerService;
        public LoanOfficerController(LoanOfficerService loanOfficerService)
        {
            _loanOfficerService = loanOfficerService;
        }

        // Action method for displaying the welcome page
        public ActionResult Welcome()
        {
            return View();
        }
        

        // Action method for displaying pending loan approvals
        public ActionResult LoanApproval()
        {
            return View(_loanOfficerService.GetDocuments());
           
        }

        //Show Rejistration Documents to the Loan Officesr
        public ActionResult GetLoanDocuments(Guid id)
        {
            TempData["registerApplicationId"] = id;
            return View(_loanOfficerService.GetAppDocuments(id));
            
        }
        public ActionResult ApproveLoan()
        {

            var id = TempData.Peek("registerApplicationId");
            _loanOfficerService.RegApproveLoan((Guid)id);
            return RedirectToAction("LoanApproval");
           
        }

        // Action method for rejecting a loan
        public ActionResult RejectLoan()
        {
            var id = TempData.Peek("registerApplicationId");
            _loanOfficerService.RejectApproveLoan((Guid)id);
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
            return View(_loanOfficerService.GetToShowCollateralDocuments(id));
            
            
        }
        // Action method for approving a collateral
        public ActionResult ApproveCollateral()
        {
            var id = TempData.Peek("collateralApplicationId");
            _loanOfficerService.ApproveCollateralDocuments((Guid)id);
            return RedirectToAction("CollateralApproval");
            
        }

        // Action method for rejecting a collateral
        public ActionResult RejectCollateral()
        {
            var id = TempData.Peek("collateralApplicationId");
            _loanOfficerService.RejectCollateralDocuments((Guid)id);
            return RedirectToAction("CollateralApproval");
            
        }
        
    }

}