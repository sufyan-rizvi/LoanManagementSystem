using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Razorpay.Api;


namespace LoanManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        // GET: RazorPayment
        private string _key = ConfigurationManager.AppSettings["RazorpayKey"];
        private string _secret = ConfigurationManager.AppSettings["RazorpaySecret"];

        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        // Create Razorpay Order
        [HttpGet]
        public JsonResult CreateOrder(string amount)
        {
            try
            {
                var client = new RazorpayClient(_key, _secret);

                // Convert the amount to paise (smallest currency unit)
                int amountInPaise = Convert.ToInt32(Convert.ToDecimal(amount) * 100);

                // Create an order in Razorpay for the amount entered by the user
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", amountInPaise }, // Amount in paise
                    { "currency", "INR" },
                    { "receipt", "order_rcptid_11" }
                };

                Order order = client.Order.Create(options);

                // Return the order ID and amount to the client-side
                return Json(new { orderId = order["id"].ToString(), amount = amountInPaise }, JsonRequestBehavior.AllowGet);
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
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}