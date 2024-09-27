using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly CloudinaryService _cloudinaryService;

        public DocumentsController()
        {
            _cloudinaryService = new CloudinaryService();
        }

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        // Upload Registration Document
        [HttpPost]
        public ActionResult UploadRegistrationDocument(Guid customerId, HttpPostedFileBase file, int documentTypeId)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.Message = "Please select a document to upload.";
                return View("Index");
            }

            var uploadResult = _cloudinaryService.UploadDocument(file);

            if (uploadResult.Error != null)
            {
                ViewBag.Message = "Error: " + uploadResult.Error.Message;
                return View("Index");
            }

            using (var session = NhibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var customer = session.Get<Customer>(customerId);

                    var document = new RegistrationDocuments
                    {
                        RegId = Guid.NewGuid(),
                        DocumentType = session.Get<DocumentType>(documentTypeId),
                        DocLink = uploadResult.SecureUrl.ToString(),
                        Customer = customer
                    };

                    session.Save(document);
                    transaction.Commit();
                }
            }

            return RedirectToAction("ViewRegistrationDocuments", new { customerId = customerId });
        }

        // Upload Collateral Document
        [HttpPost]
        public ActionResult UploadCollateralDocument(Guid customerId, HttpPostedFileBase file, int documentTypeId)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.Message = "Please select a document to upload.";
                return View("Index");
            }

            var uploadResult = _cloudinaryService.UploadDocument(file);

            if (uploadResult.Error != null)
            {
                ViewBag.Message = "Error: " + uploadResult.Error.Message;
                return View("Index");
            }

            using (var session = NhibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var customer = session.Get<Customer>(customerId);

                    var document = new CollateralDocuments
                    {
                        CollateralId = Guid.NewGuid(),
                        DocumentType = session.Get<DocumentType>(documentTypeId),
                        DocLink = uploadResult.SecureUrl.ToString(),
                        Customer = customer
                    };

                    session.Save(document);
                    transaction.Commit();
                }
            }

            return RedirectToAction("ViewCollateralDocuments", new { customerId = customerId });
        }

        // View Registration Documents
        public ActionResult ViewRegistrationDocuments(Guid customerId)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var documents = session.Query<RegistrationDocuments>()
                                       .Where(d => d.Customer.CustId == customerId)
                                       .ToList();

                return View(documents);
            }
        }

        // View Collateral Documents
        public ActionResult ViewCollateralDocuments(Guid customerId)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var documents = session.Query<CollateralDocuments>()
                                       .Where(d => d.Customer.CustId == customerId)
                                       .ToList();

                return View(documents);
            }
        }
    }



}