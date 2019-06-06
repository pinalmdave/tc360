using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace TechScreen.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public JsonResult SetPaymentAmount(decimal stripePaymentAmount)
        {
            ViewBag.PaymentAmount = stripePaymentAmount;

            return Json("Success");
        }

        [HttpPost]
        public IActionResult PaymentProcessing(string stripeEmail, string stripeToken)
        {  
            var customerOptions = new CustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            };

            var customerService = new CustomerService();
            Customer customer = customerService.Create(customerOptions);

            // Charge the Customer instead of the card:
            //var chargeOptions = new ChargeCreateOptions
            //{
            //    Amount = 1000,
            //    Currency = "usd",
            //    CustomerId = customer.Id,
            //};
            //var chargeService = new ChargeService();
            //Charge charge = chargeService.Create(chargeOptions);

            // YOUR CODE: Save the customer ID and other info in a database for later.
            //Pinal : ToDo

            // When it's time to charge the customer again, retrieve the customer ID.
            //Pinal: So save customer.Id in database and execute below method when screening is done
            var options = new ChargeCreateOptions
            {
                Amount = 1500, // $15.00 this time
                Currency = "usd",
                CustomerId = customer.Id, // Previously stored, then retrieved
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            return View();
        }


        //[HttpPost]
        //public IActionResult Processing(string stripeToken, string stripeEmail)
        //{
        //    Dictionary<string, string> Metadata = new Dictionary<string, string>();
        //    Metadata.Add("Product", "ScreenCredit");
        //    Metadata.Add("Quantity", "10");
        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = amount,
        //        Currency = "USD",
        //        Description = "Buying 10 rubber ducks",
        //        SourceId = stripeToken,
        //        ReceiptEmail = stripeEmail,
        //        Metadata = Metadata
        //    };
        //    var service = new ChargeService();
        //    Charge charge = service.Create(options);
        //    return View();
        //}
    }
}