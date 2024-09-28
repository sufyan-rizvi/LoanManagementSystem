using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;

namespace LoanManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly ICustomerService _schemeService;

        public CustomerController(ICustomerService schemeService)
        {
            _cloudinaryService = new CloudinaryService();
            _schemeService = schemeService;
        }

        public ActionResult Schemes()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var schemes = _schemeService.GetAllSchemes();
                return View(schemes);
            }
        }
        public ActionResult Index()
        {
            var customer = (Customer)Session["Customer"];
            var loans = _schemeService.CustomerApplications(customer.CustId);
            return View(loans);
        }


        public ActionResult CustomerLoanSchemes()
        {
            var customer = (Customer)Session["Customer"];
            return View(customer);
        }
        public ActionResult ApprovalDocIndex()
        {
            return View();
        }
        public ActionResult ColateralDocIndex()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadApprovalDoc(List<HttpPostedFileBase> file)
        {
            for (var i = 1; i <= file.Count(); i++)
            {
                if (file == null || file[i].ContentLength == 0)
                {
                    ViewBag.Message = "Please select a file to upload.";
                    return View("ApprovalDocIndex");
                }

                var result = _cloudinaryService.UploadImage(file[i]);

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

            //if (file == null || file[1].ContentLength == 0)
            //{
            //    ViewBag.Message = "Please select a file to upload.";
            //    return View("ApprovalDocIndex");
            //}

            //var result = _cloudinaryService.UploadImage(file[1]);

            //// Check if the result contains an error
            //if (result.Error != null)
            //{
            //    ViewBag.Message = "Error: " + result.Error.Message;
            //    return View("ApprovalDocIndex");
            //}


            //// Proceed with the result if there is no error

            //ViewBag.ImageUrl = result.SecureUrl.ToString();
            //ViewBag.PublicId = result.PublicId;

            //return View("UploadResult");
        }

        [HttpPost]
        public ActionResult UploadCollateralDoc(List<HttpPostedFileBase> file)
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
            for (var i = 1; i <= file.Count(); i++)
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
            // Proceed with the result if there is no error
            //ViewBag.ImageUrl = result.SecureUrl.ToString();
            //ViewBag.PublicId = result.PublicId;

            return View("UploadResult");
        }
        public ActionResult ShowImage(string publicId)
        {
            string cloudinaryUrl = $"https://res.cloudinary.com/{System.Configuration.ConfigurationManager.AppSettings["CloudinaryCloudName"]}/image/upload/{publicId}";
            return Redirect(cloudinaryUrl);
        }
    }
}