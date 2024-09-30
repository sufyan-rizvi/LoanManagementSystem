using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using LoanManagementSystem.ViewModels;

namespace LoanManagementSystem.Controllers
{
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
            return View();
        }
        [HttpPost]
        public ActionResult AddCollateral(List<HttpPostedFileBase> file)
        {
            if (file == null || file[1].ContentLength == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View("ColateralDocIndex");
            }

            var result = _cloudinaryService.UploadImage(file[1]);

            // Check if the result contains an error
            if (result.Error != null)
            {
                ViewBag.Message = "Error: " + result.Error.Message;
                return View("ColateralDocIndex");
            }
            for (var i = 0; i <= file.Count(); i++)
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

                        };

                        session.Save(collateralDocs);
                        transaction.Commit();

                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult ApplyLoan(Guid id)
        {
            var scheme = new LoanApplicationSchemeVM();
            scheme.LoanScheme = _customerService.GetSchemeById(id);
            return View(scheme);
        }

        public JsonResult AddLoanDetails(LoanApplicationSchemeVM vm)
        {
            var application = vm.LoanApplication;
            application.Scheme = _customerService.GetSchemeById(vm.LoanScheme.LoanSchemeId);


            application.Applicant = (Customer)Session["Customer"];

            var officerList = _adminService.GetAllOfficers();
            Random number = new Random(); ;
            application.AssignedOfficer = officerList[number.Next(0, officerList.Count)];

            application.ApplicationDate = DateTime.Now;

            _customerService.AddLoanApplication(application);
            return Json("Nice");
        }

        [HttpPost]
        public ActionResult UploadDoc(List<HttpPostedFileBase> files)
        {
            for (var i = 0; i < files.Count(); i++)
            {
                if (files == null || files[i].ContentLength == 0)
                {
                    ViewBag.Message = "Please select a file to upload.";
                    return View("ApprovalDocIndex");
                }

                var result = _cloudinaryService.UploadImage(files[i]);

                // Check if the result contains an error
                if (result.Error != null)
                {
                    ViewBag.Message = "Error: " + result.Error.Message;
                    return View("ApprovalDocIndex");
                }

                using (var session = NhibernateHelper.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var approvalDocs = new RegistrationDocuments()
                        {

                            DocumentType = (DocumentType)i,
                            PublicId = result.PublicId,
                            ImageUrl = result.SecureUrl.ToString(),

                        };

                        session.Save(approvalDocs);
                        transaction.Commit();

                    }
                }
            }

            return View("UploadResult");

        }
        public ActionResult ShowImage(string publicId)
        {
            string cloudinaryUrl = $"https://res.cloudinary.com/{System.Configuration.ConfigurationManager.AppSettings["CloudinaryCloudName"]}/image/upload/{publicId}";
            return Redirect(cloudinaryUrl);
        }
    }
}