using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetData(int page, int rows, string sidx, string sord, bool _search,
           string searchField, string searchString, string searchOper)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                
                var detailList = session.Query<LoanOfficer>().ToList();

                //if (_search && searchField == "Name" && searchOper == "eq")
                //{
                //    detailList = detailList.Where(q => q.Email == searchString).ToList();
                //}


                //Get total count of records 
                int totalCount = detailList.Count();

                //Calculate total Pages
                int totalPages = (int)Math.Ceiling((double)totalCount / rows);

                //switch (sidx)
                //{
                //    case "PhoneNumber":
                //        detailList = sord == "asc" ? detailList.OrderBy(p => p.PhoneNumber).ToList()
                //            : detailList.OrderByDescending(p => p.PhoneNumber).ToList();
                //        break;

                //    case "Email":
                //        detailList = sord == "asc" ? detailList.OrderBy(p => p.Email).ToList()
                //            : detailList.OrderByDescending(p => p.Email).ToList();
                //        break;

                //    default:
                //        break;

                //}



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
                        p.User.FirstName,
                        p.User.LastName
                        }
                    }).Skip((page - 1) * rows).Take(rows).ToArray()
                };
                
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
    }
}