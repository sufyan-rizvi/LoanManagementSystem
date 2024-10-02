using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Service;
using LoanManagementSystem.ViewModels;
using NHibernate.Linq;

using BC = BCrypt.Net.BCrypt;
using System.Threading.Tasks;
using AutoMapper;
using LoanManagementSystem.DTOs;

namespace LoanManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public ActionResult Analytics()
        {
            //bhai jii we will send these data from our DataBase accordingly
            var loanApplications = new List<int> { 12, 19, 3, 5, 2 };  // Example data
            var loanAmounts = new List<int> { 5000, 10000, 7500, 12000, 9000 };  // Data for line chart
            var months = new List<string> { "January", "February", "March", "April", "May" };  // X-axis labels
                                                                                               // Data for NPA (example: percentage of NPAs each month)
            var npaPercentages = new List<int> { 5, 7, 6, 8, 10 };


            ViewBag.LoanApplications = loanApplications;
            ViewBag.LoanAmounts = loanAmounts;
            ViewBag.Months = months;
            ViewBag.NPAPercentages = npaPercentages;




            return View();
        }

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
            _adminService.EditOfficer(officer);
            return Json(new { success = true, message = "Officer details updated!" });
        }

        public JsonResult GetAllSchemes()
        {
            var schemes = _adminService.GetAllSchemes();
            var dto = Mapper.Map<List<LoanSchemeDTO>>(schemes);
            return Json(dto, JsonRequestBehavior.AllowGet);
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