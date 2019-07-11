using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using TechScreen.Services;

namespace TechScreen.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private IScreeningRepository screeningRepository;

        public PaymentController(IScreeningRepository _screeningRepository)
        {
            this.screeningRepository = _screeningRepository;
        }

        public IActionResult MakePayment(int id)
        {
            var transactionModel = this.screeningRepository.GetTransaction(id);

            TempData["TransactionId"] = id;

            var amount = Convert.ToInt32(transactionModel.AmountBilled);

            string sessionId = GetStripeSessionId(amount);

            ViewBag.SessionId = sessionId;

            return View();
        }

        private string GetStripeSessionId(int amount)
        {
            StripeConfiguration.SetApiKey("sk_test_908vWm5ejJGpuZ7AIpcyFfMh00DY9KOyeQ");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
            "card",
            },
                LineItems = new List<SessionLineItemOptions> {
                new SessionLineItemOptions {
                    Name = "TechScreen360",
                    Description = "Candidate Screening",
                    Amount = amount,
                    Currency = "usd",
                    Quantity = 1,
                },
            },
                SuccessUrl = "https://www.techscreen360.com/Payment/Success",
                CancelUrl = "https://www.techscreen360.com/Payment/Error",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Id;
        }

        public IActionResult Success()
        {
            var transactionId = Convert.ToInt32(TempData["TransactionId"]);
            this.screeningRepository.UpdateTransaction(transactionId, "Success");
            return View();
        }

        public IActionResult Error()
        {
            var transactionId = Convert.ToInt32(TempData["TransactionId"]);
            this.screeningRepository.UpdateTransaction(transactionId, "Failed");
            return View();
        }

        //public JsonResult SetPaymentAmount(decimal stripePaymentAmount)
        //{
        //    ViewBag.PaymentAmount = stripePaymentAmount;

        //    return Json("Success");
        //}

        //[HttpPost]
        //public IActionResult PaymentProcessing(string stripeEmail, string stripeToken)
        //{  
        //    var customerOptions = new CustomerCreateOptions
        //    {
        //        Email = stripeEmail,
        //        SourceToken = stripeToken
        //    };

        //    var customerService = new CustomerService();
        //    Customer customer = customerService.Create(customerOptions);

        //    // Charge the Customer instead of the card:
        //    //var chargeOptions = new ChargeCreateOptions
        //    //{
        //    //    Amount = 1000,
        //    //    Currency = "usd",
        //    //    CustomerId = customer.Id,
        //    //};
        //    //var chargeService = new ChargeService();
        //    //Charge charge = chargeService.Create(chargeOptions);

        //    // YOUR CODE: Save the customer ID and other info in a database for later.
        //    //Pinal : ToDo

        //    // When it's time to charge the customer again, retrieve the customer ID.
        //    //Pinal: So save customer.Id in database and execute below method when screening is done
        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = 1500, // $15.00 this time
        //        Currency = "usd",
        //        CustomerId = customer.Id, // Previously stored, then retrieved
        //    };
        //    var service = new ChargeService();
        //    Charge charge = service.Create(options);

        //    return View();
        //}


        //[HttpPost]
        //public IActionResult Processing(string stripeToken, string stripeEmail)
        //{
        //    Dictionary<string, string> Metadata = new Dictionary<string, string>();
        //    Metadata.Add("Product", "Screening");
        //    Metadata.Add("Quantity", "11");
        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = 100,
        //        Currency = "USD",
        //        Description = "Buying 11 screening",
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