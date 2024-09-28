using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers
{
    //[Authorize(Roles = "User")]
    public class LoanSchemeController : Controller
    {
        // Action to list all loan schemes with short description
        public ActionResult ListSchemes()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var schemes = session.Query<LoanScheme>().ToList();
                return View(schemes);
            }
            
        }

        // Action to show the full details of a loan scheme
        public ActionResult SchemeDetails(int id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                var scheme = session.Get<LoanScheme>(id);
                if (scheme == null)
                {
                    return HttpNotFound();
                }
                return View(scheme);
            }
        }
        public void SaveLoanScheme(LoanScheme loanScheme)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(loanScheme);
                    txn.Commit();
                }
            }
        }

        public IList<LoanScheme> GetAllLoanSchemes()
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                return session.Query<LoanScheme>().ToList();
            }
        }

        public LoanScheme GetLoanSchemeById(int id)
        {
            using (var session = NhibernateHelper.CreateSession())
            {
                return session.Get<LoanScheme>(id);
            }
        }


    }

}