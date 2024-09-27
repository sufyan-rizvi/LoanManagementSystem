using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers
{
    public class EMICalculatorController : Controller
    {
        // GET: EMICalculator
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateEMI(EMICalculatorModel model)
        {
            if (ModelState.IsValid)
            {
                model.EMIAmount = CalculateEMI(model.LoanAmount, model.InterestRate, model.LoanTenure);
            }
            return View("Index", model);
        }

        private decimal CalculateEMI(decimal loanAmount, decimal annualInterestRate, int loanTenure)
        {
            decimal monthlyInterestRate = (annualInterestRate / 100) / 12;
            decimal emi = loanAmount * monthlyInterestRate * (decimal)Math.Pow((double)(1 + monthlyInterestRate), loanTenure)
                          / (decimal)(Math.Pow((double)(1 + monthlyInterestRate), loanTenure) - 1);

            return Math.Round(emi, 2);
        }
    }

}