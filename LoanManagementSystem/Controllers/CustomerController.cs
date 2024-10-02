using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using LoanManagementSystem.ViewModels;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace LoanManagementSystem.Controllers
{
    [Authorize(Roles = "Customer")]
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
        public ActionResult Schemes()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var schemes = _customerService.GetAllSchemes();
                return View(schemes);
            }
        }
        public ActionResult Index()
        {
            var customer = (Customer)Session["Customer"];
            var loans = _customerService.CustomerApplications(customer.CustId);
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
            existingApplication.Status = ApplicationStatus.CollateralApproved;
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

        [HttpPost]
        public ActionResult UploadDoc(List<HttpPostedFileBase> files)
        {
            //var existingApplication = _customerService.GetApplicationById(application.ApplicationId);
            for (var i = 0; i < files.Count(); i++)
            {

                var result = _cloudinaryService.UploadImage(files[i]);

                if (result.Error != null)
                {
                    throw new InvalidOperationException("Unable to Upload images at the moment!");
                }

                using (var session = NhibernateHelper.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //var approvalDocs = new RegistrationDocuments()
                        //{

                        //    DocumentType = (DocumentType)i,
                        //    PublicId = result.PublicId,
                        //    ImageUrl = result.SecureUrl.ToString(),
                        //    Customer = (Customer)Session["Customer"]
                        //};

                        //session.Save(approvalDocs);
                        transaction.Commit();

                    }
                }
            }
            return Json("Great Success!");


        }
        public ActionResult ShowImage(string publicId)
        {
            string cloudinaryUrl = $"https://res.cloudinary.com/{System.Configuration.ConfigurationManager.AppSettings["CloudinaryCloudName"]}/image/upload/{publicId}";
            return Redirect(cloudinaryUrl);
        }
    }
}