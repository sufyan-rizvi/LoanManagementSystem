﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using AutoMapper;
using LoanManagementSystem.DTOs;
using NHibernate.Transform;
using NHibernate;
using System.Web.UI.WebControls;
using Rotativa;
using System.ServiceModel.Channels;
using static System.Collections.Specialized.BitVector32;
using PagedList;
using PagedList.Mvc;

namespace LoanManagementSystem.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        public AdminController(IAdminService adminService, ICustomerService customerService, IAccountService accountService)
        {
            _adminService = adminService;
            _customerService = customerService;
            _accountService = accountService;
        }
        public ActionResult Analytics()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var result = session.CreateQuery(@"
        select 
            month(ApplicationDate) as Month, 
            year(ApplicationDate) as Year, 
            count(*) as ApplicationCount 
        from LoanApplication 
        group by 
            month(ApplicationDate), 
            year(ApplicationDate)
        order by 
            Year, Month")
                    .SetResultTransformer(Transformers.AliasToBean<LoanApplicationMonthWise>())
                    .List<LoanApplicationMonthWise>();

                var loanApplications = new Int64[12];
                foreach (var item in result)
                {
                    // Assuming item.Month is 1 for January, 2 for February, etc.
                    loanApplications[item.Month - 1] = item.ApplicationCount; // -1 to adjust for zero-based indexing
                }

                //bhai jii we will send these data from our DataBase accordingly
                // Example data

                var months = new List<string>
{
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
};  // X-axis labels
    // Data for NPA (example: percentage of NPAs each month)

                var npaResult = session.CreateQuery(@"
        select 
            month(ApplicationDate) as Month, 
            year(ApplicationDate) as Year, 
            count(*) as ApplicationCount 
        from LoanApplication 
        where Status = :status
        group by 
            month(ApplicationDate), 
            year(ApplicationDate)
        order by 
            Year, Month")
        .SetParameter("status", ApplicationStatus.NPA) // Set the status parameter
        .SetResultTransformer(Transformers.AliasToBean<LoanApplicationMonthWise>())
        .List<LoanApplicationMonthWise>();


                var npaPercentages = new Int64[12];
                foreach (var item in npaResult)
                {
                    // Assuming item.Month is 1 for January, 2 for February, etc.
                    npaPercentages[item.Month - 1] = item.ApplicationCount; // -1 to adjust for zero-based indexing
                }


                var amountResult = session.CreateQuery(@"
        select 
            month(ApplicationDate) as Month, 
            year(ApplicationDate) as Year, 
            count(*) as ApplicationCount, 
            sum(LoanAmount) as TotalLoanAmount
        from LoanApplication 
        where Status = :status
        group by 
            month(ApplicationDate), 
            year(ApplicationDate)
        order by 
            Year, Month")
        .SetParameter("status", ApplicationStatus.NPA)
        .SetResultTransformer(Transformers.AliasToBean<LoanApplicationMonthWise>())
        .List<LoanApplicationMonthWise>();



                var loanAmounts = new Int64[12];  // Data for line chart

                foreach (var item in amountResult)
                {
                    // Assuming item.Month is 1 for January, 2 for February, etc.
                    loanAmounts[item.Month - 1] = (Int64)item.TotalLoanAmount; // -1 to adjust for zero-based indexing
                }

                int officerCount = _adminService.GetAllActiveOfficers().Count();
                int customerCount = session.Query<Customer>().Count();
                int adminCount = session.Query<Admin>().Count();







                ViewBag.OfficerCount = officerCount;
                ViewBag.CustomerCount = customerCount;
                ViewBag.AdminCount = adminCount;
                ViewBag.LoanApplications = loanApplications;
                ViewBag.LoanAmounts = loanAmounts;
                ViewBag.Months = months;
                ViewBag.NPAPercentages = npaPercentages;




                return View();
            }
        }


        public ActionResult GeneratePDf()
        {
            return new Rotativa.ActionAsPdf("Analytics");
        }

        //
        //
        //
        //
        //
        //
        //

        // Action to display the report view
        [AllowAnonymous]
        public ActionResult LoanReport()
        {
            return View(_adminService.LoanReport());

        }

        public ActionResult NPAReport()
        {
            return View(_adminService.NPAReport());

        }

        public ActionResult SchemesReport()
        {
            return View(_adminService.SchemesReportk());

        }
        public ActionResult LoanOfficerReport()
        {
            return View(_adminService.LoanOfficerReport());

        }


        // Action to generate the PDF
        public ActionResult DownloadReport()
        {
             var dto = _adminService.LoanReport();

                return new ViewAsPdf("LoanReport", dto)
                {
                    FileName = "Loan Report.pdf",
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4
                };
            
        }


        public ActionResult DownloadNPAReport()
        {
            
                var dto = _adminService.NPAReport();

                return new ViewAsPdf("NPAReport", dto)
                {
                    FileName = "Loan NPA Report.pdf",
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4
                };
            
        }

        public ActionResult DownloadSchemeReport()
        {
            
                var dto = _adminService.SchemesReportk();

                return new ViewAsPdf("SchemesReport", dto)
                {
                    FileName = "Loan Schemes Report.pdf",
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4
                };
            
        }

        public ActionResult DownloadLoanOfficerReport()
        {
            
                var dto = _adminService.LoanOfficerReport();

                return new ViewAsPdf("LoanOfficerReport", dto)
                {
                    FileName = "Loan Officers Report.pdf",
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4
                };
            
        }


        //
        //
        //
        //
        //
        //
        //








        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int page, int rows, string sidx, string sord, bool _search,
           string searchField, string searchString, string searchOper)
        {


            var detailList = _adminService.GetAllOfficers();

            if (_search && searchField == "User.FirstName" && searchOper == "eq")
            {
                detailList = detailList.Where(q => q.User.FirstName == searchString).ToList();
            }
            else if (_search && searchField == "User.LastName" && searchOper == "eq")
            {
                detailList = detailList.Where(q => q.User.FirstName == searchString).ToList();
            }
            else if (_search && searchField == "User.Email" && searchOper == "eq")
            {
                detailList = detailList.Where(q => q.User.FirstName == searchString).ToList();
            }
            else if (_search && searchField == "User.PhoneNumber" && searchOper == "eq")
            {
                detailList = detailList.Where(q => q.User.FirstName == searchString).ToList();
            }
            else if (_search && searchField == "User.Age" && searchOper == "eq")
            {
                detailList = detailList.Where(q => q.User.Age.ToString() == searchString).ToList();
            }



            //Get total count of records 
            int totalCount = detailList.Count();

            //Calculate total Pages
            int totalPages = (int)Math.Ceiling((double)totalCount / rows);

            switch (sidx)
            {
                case "User.PhoneNumber":
                    detailList = sord == "asc" ? detailList.OrderBy(p => p.User.PhoneNumber).ToList()
                        : detailList.OrderByDescending(p => p.User.PhoneNumber).ToList();
                    break;

                case "User.Email":
                    detailList = sord == "asc" ? detailList.OrderBy(p => p.User.Email).ToList()
                        : detailList.OrderByDescending(p => p.User.Email).ToList();
                    break;

                case "User.FirstName":
                    detailList = sord == "asc" ? detailList.OrderBy(p => p.User.FirstName).ToList()
                        : detailList.OrderByDescending(p => p.User.FirstName).ToList();
                    break;

                case "User.LastName":
                    detailList = sord == "asc" ? detailList.OrderBy(p => p.User.LastName).ToList()
                        : detailList.OrderByDescending(p => p.User.LastName).ToList();
                    break;
                case "User.Age":
                    detailList = sord == "asc" ? detailList.OrderBy(p => p.User.Age).ToList()
                        : detailList.OrderByDescending(p => p.User.Age).ToList();
                    break;

                default:
                    break;

            }



            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalCount,

                rows = detailList.Select(p => new
                {

                    cell = new string[]
                    {
                        p.OfficerId.ToString(),
                        p.User.UserId.ToString(),
                        p.User.FirstName,
                        p.User.LastName,
                        p.User.Email,
                        p.User.PhoneNumber,
                        p.User.Age.ToString(),
                        p.User.Address.FlatNo + ", " + p.User.Address.BuildingName + ", " + p.User.Address.StreetName + ", " + p.User.Address.City + " - " +
                        p.User.Address.PinCode + ", " + p.User.Address.State + ", " + p.User.Address.Country,
                        p.User.IsActive.ToString(),
                        p.User.Username,
                        p.User.Password,
                        p.User.Address.FlatNo,
                        p.User.Address.BuildingName,
                        p.User.Address.StreetName,
                        p.User.Address.City,
                        p.User.Address.PinCode,
                        p.User.Address.State,
                        p.User.Address.Country
                    }
                }).Skip((page - 1) * rows).Take(rows).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Add(LoanOfficer officer)
        {
            officer.User.Age = Convert.ToInt32(officer.User.Age);
            if (_accountService.CheckUserNameFound(officer.User.Username))
                return Json(new { success = false, message = "Officer with same username exists!" });

            if (_accountService.CheckEmailFound(officer.User.Email))
                return Json(new { success = false, message = "Officer with same email exists!" });

            _adminService.AddOfficer(officer);
            return Json(new { success = true, message = "Officer added successfully" });
        }

        public ActionResult DeleteUpdate(Guid id)
        {
            var Officer = _adminService.ToggleOfficerDelete(id);
            if (Officer.User.IsActive)
                return Json(new { success = true, message = "Officer Reactivated successfully" });
            return Json(new { success = true, message = "Officer Deleted successfully" });

        }

        public ActionResult Edit(LoanOfficer officer)
        {
            officer.User.Age = Convert.ToInt32(officer.User.Age);
            if (_accountService.CheckUserNameFound(officer.User.Username))
                return Json(new { success = false, message = "Officer with same username exists!" });

            if (_accountService.CheckEmailFound(officer.User.Email))
                return Json(new { success = false, message = "Officer with same email exists!" });

            _adminService.EditOfficer(officer);
            return Json(new { success = true, message = "Officer details updated!" });
        }

        public JsonResult GetAllSchemes(int? i)
        {
            var schemes = _adminService.GetAllSchemes();
            var dto = Mapper.Map<List<LoanSchemeDTO>>(schemes).ToPagedList(i ?? 1, 2);

            var result = new
            {
                Schemes = dto,
                Pagination = new
                {
                    dto.PageNumber,
                    dto.PageSize,
                    dto.TotalItemCount,
                    dto.PageCount,
                    dto.HasPreviousPage,
                    dto.HasNextPage,
                    dto.IsFirstPage,
                    dto.IsLastPage
                }
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddScheme(LoanScheme scheme)
        {
            _adminService.AddScheme(scheme);
            return Json("Scheme Added Successfully!");
        }

        [HttpPost]
        public JsonResult DeleteScheme(Guid id)
        {
            var scheme = _adminService.DeleteScheme(id);
            if (scheme.IsActive)
                return Json("Scheme Reactivated!");
            return Json("Scheme Deleted!");
        }

        [HttpPost]
        public JsonResult EditScheme(LoanScheme scheme)
        {
            _adminService.EditScheme(scheme);
            return Json("Scheme Edited Successfully!");
        }

        public JsonResult GetSchemeTypes()
        {
            var schemeTypes = Enum.GetValues(typeof(SchemeType))
                          .Cast<SchemeType>()
                          .Select(e => new
                          {
                              Value = (int)e,
                              Text = e.ToString()
                          });

            return Json(schemeTypes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchemeById(Guid id)
        {
            var scheme = _adminService.GetSchemeById(id);
            return Json(scheme, JsonRequestBehavior.AllowGet);
        }
    }
}