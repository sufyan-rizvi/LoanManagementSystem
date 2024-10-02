using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Razorpay.Api;


namespace LoanManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        // GET: RazorPayment
        private string _key = ConfigurationManager.AppSettings["RazorpayKey"];
        private string _secret = ConfigurationManager.AppSettings["RazorpaySecret"];

        // GET: Payment
        public ActionResult Index(string applicationId)
        {
            

            using (var s = NhibernateHelper.CreateSession())
            {
                var application = s.Query<LoanApplication>().FirstOrDefault(l => l.ApplicationId == new Guid(applicationId));
                Session["application"] = application;
                return View(application);

            }
                
        }

        // Create Razorpay Order
        [HttpGet]
        public JsonResult CreateOrder(string amount)
        {
            try
            {
                Session["amount"] = amount;
                var client = new RazorpayClient(_key, _secret);

                // Convert the amount to paise (smallest currency unit)
                int amountInCents = Convert.ToInt32(Convert.ToDecimal(amount) * 100);

                // Create an order in Razorpay for the amount entered by the user
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", amountInCents }, // Amount in paise
                    { "currency", "INR" },
                    { "receipt", "order_rcptid_11" }
                };

                Order order = client.Order.Create(options);

                // Return the order ID and amount to the client-side
                return Json(new { orderId = order["id"].ToString(), amount = amountInCents }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Payment/ProcessPayment
        [HttpPost]
        public ActionResult ProcessPayment(string rzp_payment_id, string rzp_order_id, string rzp_signature)
        {
            try
            {
                Session["rzp_order_id"] = rzp_order_id;
                var client = new RazorpayClient(_key, _secret);

                // Create the parameters for payment verification
                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "razorpay_payment_id", rzp_payment_id },
                    { "razorpay_order_id", rzp_order_id },
                    { "razorpay_signature", rzp_signature }
                };

                // Verify the payment signature
                Utils.verifyPaymentSignature(attributes);

                // Redirect to success page if the payment is successful
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Success()
        {
            
            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var payment = new Repayment
                    {
                        PaymentDate = DateTime.Now,
                        Amount = Convert.ToDouble(Session["amount"]),
                        TransactionId = (string)Session["rzp_order_id"],
                        IsApproved = true,
                        Application = (LoanApplication)Session["application"]

                    };
                    s.Save(payment);
                    txn.Commit();
                }

            }

            return View();
        }



        public ActionResult Error()
        {
            using (var s = NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var payment = new Repayment
                    {
                        PaymentDate = DateTime.Now,
                        Amount = (double)Session["amount"],
                        TransactionId = (string)Session["rzp_order_id"],
                        IsApproved = false
                    };
                    s.Save(payment);
                    txn.Commit();
                }

            }
            return View();
        }

        public ActionResult UpdateApplicationAfterPayment()
        {
            
            using(var s =  NhibernateHelper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var application = (LoanApplication)Session["application"];
                    application.PaymentsMade += 1;
                    if(application.PaymentsMade >= application.Tenure) 
                        application.Status = ApplicationStatus.LoanClosed;


                    application.NextPaymentDate = application.NextPaymentDate.AddMonths(1);

                    s.Merge(application);
                    txn.Commit();
                }
            }
            return Json("YAY",JsonRequestBehavior.AllowGet);
        }


    }
}