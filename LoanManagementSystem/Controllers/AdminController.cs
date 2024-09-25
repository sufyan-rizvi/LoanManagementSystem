using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using NHibernate.Linq;

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
                        p.User.Address.FullAddress,
                        p.User.IsActive.ToString(),
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
                    address.User = user;
                    user.IsActive = true;
                    officer.User = user;
                    officer.User.Address = address;                    
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
                    var Officer = s.Query<LoanOfficer>().FirstOrDefault();
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
                    //var existingProduct = s.Query<LoanOfficer>().FirstOrDefault(p => p.Id == detail.Id);
                    //if (existingProduct != null)
                    //{
                    //    existingProduct.PhoneNumber = detail.PhoneNumber;
                    //    existingProduct.Email = detail.Email;
                    //    s.Update(existingProduct);
                    //    txn.Commit();
                    //}
                }
            }
            return Json(new { success = true, message = "Product edited successfully." });




        }
    }
}