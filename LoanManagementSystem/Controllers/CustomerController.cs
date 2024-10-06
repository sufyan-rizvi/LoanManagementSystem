using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using LoanManagementSystem.ViewModels;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;

namespace LoanManagementSystem.Controllers
{
    //[Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly ICustomerService _customerService;
        private readonly IAdminService _adminService;


        public CustomerController(ICustomerService customerService, IAdminService adminService)
        {
            _cloudinaryService = new CloudinaryService();
            _customerService = customerService;
            _adminService = adminService;
        }
        [AllowAnonymous]
        public ActionResult EMIcal()
        {
            return View();
        }
        public ActionResult CalculateEMIForView(LoanScheme scheme, LoanApplication application)
        {
            var interestRate = _adminService.GetSchemeById(scheme.LoanSchemeId).InterestRate / 12 / 100;
            var tenure = application.Tenure;
            var amount = application.LoanAmount;
            var factor = Math.Pow((1 + interestRate), tenure);
            var emi = (amount * interestRate * factor) / (factor - 1);

            return Json(Math.Max(0, emi), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAllPayments(Guid id)
        {
            var payments = _customerService.GetAllPaymentsForApplication(id);
            return View(payments);
        }

        public ActionResult GeneratePDF(Guid id)
        {
            return new Rotativa.ActionAsPdf("Getallpayments", new { id = id })
            {
                FileName = "PaymentsReport.pdf" // Set the desired PDF filename
            };
        }


        [AllowAnonymous]
        public ActionResult Schemes(int? i)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var schemes = _customerService.GetAllSchemes().ToPagedList(i?? 1,4);
                return View(schemes);
            }
        }
        public ActionResult Index(int? i)
        {
            var customer = (Customer)Session["Customer"];
            var loans = (_customerService.CustomerApplications(customer.CustId)).ToPagedList(i ?? 1, 3);
            return View(loans);
        }

        public ActionResult AddCollateral(Guid applicationId)
        {
            var application = _customerService.GetApplicationById(applicationId);
            return View(application);
        }

        [HttpPost]
        public ActionResult AddCollateral(LoanApplication application, List<HttpPostedFileBase> files)
        {

            if (files == null || files[1].ContentLength == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View();
            }
            var existingApplication = _customerService.GetApplicationById(application.ApplicationId);
            var result = _cloudinaryService.UploadImage(files[1]);

            // Check if the result contains an error
            if (result.Error != null)
            {
                ViewBag.Message = "Error: " + result.Error.Message;
                return View("ColateralDocIndex");
            }
            for (var i = 0; i < files.Count(); i++)
            {
                using (var session = NhibernateHelper.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var collateralDocs = new CollateralDocuments()
                        {

                            DocumentType = (DocumentType)(i + 3),
                            PublicId = result.PublicId,
                            ImageUrl = result.SecureUrl.ToString(),
                            Application = existingApplication
                        };

                        session.Save(collateralDocs);
                        transaction.Commit();

                    }
                }
            }
            existingApplication.Status = ApplicationStatus.CollateralPending;
            _customerService.AddLoanApplication(existingApplication);

            return RedirectToAction("Index");
        }

        public ActionResult ApplyLoan(Guid id)
        {
            var application = new LoanApplicationSchemeVM();
            application.LoanApplication = new LoanApplication();

            var customer = (Customer)Session["Customer"];
            application.LoanApplication.Applicant = customer;
            ViewData["loanaapl"] = application.LoanApplication;
            application.LoanScheme = _customerService.GetSchemeById(id);
            return View(application);
        }

        public JsonResult AddLoanDetails(FormCollection form)
        {

            string loanApplicationJson = form["LoanApplication"];
            string loanSchemeJson = form["LoanScheme"];

            var application = JsonConvert.DeserializeObject<LoanApplication>(loanApplicationJson);
            var LoanScheme = JsonConvert.DeserializeObject<LoanScheme>(loanSchemeJson);

            var files = Request.Files;


            application.Scheme = _customerService.GetSchemeById(LoanScheme.LoanSchemeId);
            var interestRate = application.Scheme.InterestRate / 12 / 100;
            var factor = (double)Math.Pow((double)(1 + interestRate), application.Tenure);
            application.EMIAmount = Math.Round(application.LoanAmount * interestRate * factor / (factor - 1), 2);




            application.RegistrationDocuments = new List<RegistrationDocuments>();

            for (var i = 0; i < files.Count; i++)
            {

                var result = _cloudinaryService.UploadImage(files[i]);

                if (result.Error != null)
                {
                    throw new InvalidOperationException("Unable to Upload images at the moment!");
                }

                var approvalDocs = new RegistrationDocuments()
                {

                    DocumentType = (DocumentType)i,
                    PublicId = result.PublicId,
                    ImageUrl = result.SecureUrl.ToString(),
                    Application = application


                };
                application.RegistrationDocuments.Add(approvalDocs);
            }

            application.Applicant = (Customer)Session["Customer"];
            application.Status = ApplicationStatus.PendingApproval;

            var officerList = _adminService.GetAllActiveOfficers();
            Random number = new Random(); ;
            application.AssignedOfficer = officerList[number.Next(0, officerList.Count)];

            application.ApplicationDate = DateTime.Now;

            _customerService.AddLoanApplication(application);
            return Json("Nice");
        }

        [AllowAnonymous]
        public ActionResult ShowImage(string publicId)
        {
            string cloudinaryUrl = $"https://res.cloudinary.com/{System.Configuration.ConfigurationManager.AppSettings["CloudinaryCloudName"]}/image/upload/{publicId}";
            return Redirect(cloudinaryUrl);
            //return Content(cloudinaryUrl);
        }
    }
}