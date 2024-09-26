using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using NHibernate.Linq;
using NHibernate.Proxy;

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

                var detailList = session.Query<LoanOfficer>().Fetch(u => u.User).ThenFetch(u=>u.Address).ToList();

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
                        p.User.LastName,
                        p.User.Email,
                        p.User.PhoneNumber,
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
        }


        public ActionResult Add(LoanOfficer officer, User user, Address address)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    officer = new LoanOfficer();
                    officer.User = user;
                    officer.User.Address.User = user;
                    officer.User.IsActive = true;                    
                    session.Save(officer);
                    txn.Commit();
                }
            }
            return Json(new { success = true, message = "Product added successfully" });
        }

        public ActionResult DeleteUpdate(Guid id)
        {
            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var Officer = s.Query<LoanOfficer>().FirstOrDefault(l=>l.OfficerId == id);
                    Officer.User.IsActive = !Officer.User.IsActive;
                    s.Update(Officer);
                    txn.Commit();
                }
            }
            return Json(new { success = true, message = "Product Deleted successfully" });
        }

        public ActionResult Edit(LoanOfficer officer)
        {
            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var existingOfficer = s.Query<LoanOfficer>().FirstOrDefault(l=>l.User.Username == officer.User.Username);
                    if (existingOfficer != null)
                    {
                        existingOfficer.User.FirstName = officer.User.FirstName;
                        existingOfficer.User.LastName = officer.User.LastName;
                        existingOfficer.User.PhoneNumber = officer.User.PhoneNumber;
                        existingOfficer.User.Address.FlatNo = officer.User.Address.FlatNo;
                        existingOfficer.User.Address.BuildingName = officer.User.Address.BuildingName;
                        existingOfficer.User.Address.StreetName = officer.User.Address.StreetName;
                        existingOfficer.User.Address.City = officer.User.Address.City;
                        existingOfficer.User.Address.PinCode = officer.User.Address.PinCode;
                        existingOfficer.User.Address.State = officer.User.Address.State;
                        existingOfficer.User.Address.Country = officer.User.Address.Country;
                        
                        s.Update(existingOfficer);
                        txn.Commit();
                    }
                }
            }
            return Json(new { success = true, message = "Product edited successfully." });




        }
    }
}