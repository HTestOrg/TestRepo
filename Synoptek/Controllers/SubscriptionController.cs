using System.Linq;
using BusinessLogic.Models;
using BusinessObjects;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Synoptek.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Broker + "," + Helpers.Constants.UserRoles.Investor + "," + Helpers.Constants.UserRoles.RegisterUser)]
    public class SubscriptionController : BaseController
    {
        #region Get Payment Subscription Popup
        public ActionResult GetSubscriptionPopup(SubscriptionOption model)
        {
            var subscriptionModel = new SubscriptionModel();
            subscriptionModel.subscriptionOption = model;
            subscriptionModel.CardType = GetPaymentCardType();
            subscriptionModel.ExpirationYearList = GetExpirationYear();
            subscriptionModel.ExpirationMonthList = GetExpirationMonth();
            var customerID = SessionController.UserSession.CustomerID;           //Check whether the user already has credit card details or not
            if (customerID != null && customerID != "")
            {
                var customerService = new StripeCustomerService();
                StripeCustomer stripeCustomer = customerService.Get(customerID);
                var cardService = new StripeCardService();
                StripeCard stripeCard = cardService.Get(customerID, stripeCustomer.DefaultSourceId); // optional isRecipient 
                foreach (var card in subscriptionModel.CardType)
                {
                    stripeCard.Brand = subscriptionModel.CardType[0].Name;
                    subscriptionModel.CardTypeID = Convert.ToInt32(subscriptionModel.CardType[0].ID);
                }
                subscriptionModel.ExpirationMonth = stripeCard.ExpirationMonth;
                subscriptionModel.ExpirationYear = stripeCard.ExpirationYear;
                subscriptionModel.CardTypeID = subscriptionModel.CardTypeID;
                subscriptionModel.CardNumber = "************" + stripeCard.Last4;
                subscriptionModel.NameOnCard = stripeCard.Name;
            }
            //----------------------
            if (model.RoleId == 2)
            {
                ViewBag.SubscriptionTitle = "Find unlimited investment opportunities for " + model.AmountPerMonth + " per month.";
            }
            else if (model.RoleId == 3)
            {
                ViewBag.SubscriptionTitle = "List unlimited investment opportunities for " + model.AmountPerMonth + " per month.";
            }
            return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
        }
        #endregion


        #region Get Subscription Options
        public ActionResult GetSubscriptionOptions(long roleID)
        {
            SubscriptionOptionsList subscriptionOption = new SubscriptionOptionsList();
            subscriptionOption.Subscriptions = GetSubscriptionList(roleID);
            double monthlyAmount = subscriptionOption.Subscriptions.Where(w => w.PlanName.Contains("Monthly")).Select(y => y.Amount).FirstOrDefault();
            double amountPerYear_Monthly = monthlyAmount * 12;
            foreach (var list in subscriptionOption.Subscriptions)
            {
                list.RoleId = roleID;
                if (list.PlanName.Contains("Monthly"))
                {
                    list.BillSavingAmount = "";
                    list.Caption = "1 MONTH";
                    list.Save = "";
                    list.AmountPerMonth = list.Amount;
                    list.BillingCycle = "every month";
                }
                if (list.PlanName.Contains("Yearly"))
                {
                    list.BillSavingAmount = "$" + amountPerYear_Monthly.ToString();
                    list.AmountPerMonth = list.Amount / 12;
                    double savePer = ((amountPerYear_Monthly - list.Amount) / amountPerYear_Monthly) * 100;
                    savePer = System.Math.Round(savePer, 2);
                    list.Caption = "1 YEAR";
                    list.Save = "SAVE " + savePer + " %";
                    list.BillingCycle = "every year";
                }
                if (list.PlanName.Contains("Semi"))
                {
                    list.BillSavingAmount = "$" + (monthlyAmount * 6).ToString();
                    list.AmountPerMonth = list.Amount / 6;
                    double savePer = ((amountPerYear_Monthly - (list.Amount * 2)) / amountPerYear_Monthly) * 100;
                    savePer = System.Math.Round(savePer, 2);
                    list.Caption = "6 MONTHS";
                    list.Save = "SAVE " + savePer + " %";
                    list.BillingCycle = "every 6 months";
                }
            }
            return PartialView("_SubscriptionOptionsPopUp", subscriptionOption);
        }
        #endregion

        #region Get Subscription Options List From Datatbase
        public List<SubscriptionOption> GetSubscriptionList(long roleID)
        {
            Serialization serialization = new Serialization();
            Subscription subscriptionBA = new Subscription();

            Hashtable HashCriteria = new Hashtable();
            string actualCriteria;

            HashCriteria.Add("roleID", roleID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);

            List<SubscriptionOption> listSubscriptionOption = new List<SubscriptionOption>();
            listSubscriptionOption = subscriptionBA.GetSubscriptionOptionsList(actualCriteria);
            return listSubscriptionOption;
        }

        #endregion

        #region Get Payment Card type
        public List<PaymentCardTypes> GetPaymentCardType()
        {
            var subscriptionBA = new Subscription();
            var lstPaymentCardType = new List<PaymentCardTypes>();
            lstPaymentCardType = subscriptionBA.GetPaymentCardTypeDetails();
            return lstPaymentCardType;
        }
        #endregion

        #region Get Expiration Month
        public List<ExpirationMonth> GetExpirationMonth()
        {
            List<ExpirationMonth> lstExpirationMonth = new List<ExpirationMonth>();
            for (int i = 1, y = 1; i <= 12; i++)
            {
                lstExpirationMonth.Add(new ExpirationMonth { ID = y, Month = Convert.ToString(i) });
                y++;
            }
            return lstExpirationMonth;
        }
        #endregion

        #region Get Expiration Year
        public List<ExpirationYear> GetExpirationYear()
        {
            List<ExpirationYear> lstExpirationYear = new List<ExpirationYear>();
            int year = DateTime.Now.Year;
            for (int i = year, y = 1; i <= year + 25; i++)
            {
                lstExpirationYear.Add(new ExpirationYear { ID = Convert.ToInt32(i), Year = Convert.ToString(i) });
                y++;
            }
            return lstExpirationYear;
        }
        #endregion

        #region Make the payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Charge(SubscriptionModel model)
        {
            var subscriptionBA = new Subscription();
            var serialization = new Serialization();
            var status = false;
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var HashCriteriaPlan = new Hashtable();
            string actualCriteriaPlan;

            string userID = Convert.ToString(SessionController.UserSession.UserId);

            SubscriptionModel subscriptionModel = new SubscriptionModel();
            subscriptionModel.CardType = GetPaymentCardType();
            subscriptionModel.ExpirationYearList = GetExpirationYear();
            subscriptionModel.ExpirationMonthList = GetExpirationMonth();

            if (model.RoleID == 2)
            {
                ViewBag.SubscriptionTitle = "Find unlimited investment opportunities for $399 per month.";
            }
            else if (model.RoleID == 3)
            {
                ViewBag.SubscriptionTitle = "List unlimited investment opportunities for $399 per month.";
            }

            if (model.Token != null)
            {
                ModelState.Remove("State");
                ModelState.Remove("BillingAddress");
                ModelState.Remove("Zip");
                ModelState.Remove("SecurityCode");

                if (!ModelState.IsValid)
                {
                    return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
                }
                //Check if the user is already a custome ron stripe or not?
                var customer_ID = Convert.ToString(SessionController.UserSession.CustomerID);
                if (customer_ID != null && customer_ID != "")
                    model.CustomerID = customer_ID;
                else
                {
                    // 1. Create customer in stripe
                    var customerID = await CreateCustomer(model.Token);
                    model.CustomerID = customerID;
                }
                // 2. Get the plans from the Plans table
                HashCriteriaPlan.Add("CycleInMonth", 1);
                actualCriteriaPlan = serialization.SerializeBinary((object)HashCriteriaPlan);

                var result = subscriptionBA.GetPlanDetails(actualCriteriaPlan);
                SubscriptionPlans subscriptionPlans = (SubscriptionPlans)(serialization.DeSerializeBinary(Convert.ToString(result)));

                var planID = subscriptionPlans.ID;
                var subscription_PlanID = subscriptionPlans.SubscriptionPlanID;
                var amount = subscriptionPlans.Amount;

                // 3. subscription aginst that plan
                var subscriptionService = new StripeSubscriptionService();
                StripeSubscription stripeSubscription = subscriptionService.Create(model.CustomerID, subscription_PlanID);

                //4. Make the payment
                model.Amount = amount;

                var chargeId = await ProcessPayment(model);

                if (chargeId != null)
                {
                    DateTime billingDate = DateTime.Now;

                    // 5. Save detals in the subscription table with amount and token of charge
                    HashCriteria.Add("Token", model.Token);
                    HashCriteria.Add("UserID", userID);
                    HashCriteria.Add("Amount", model.Amount);
                    HashCriteria.Add("BillingDate", Convert.ToString(billingDate.ToString("dd/MM/yyyy")));
                    HashCriteria.Add("CustomerID", model.CustomerID);
                    HashCriteria.Add("PlanID", planID);
                    HashCriteria.Add("SubscriptionID", stripeSubscription.Id);
                    HashCriteria.Add("ChargeID", chargeId);

                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);

                    var subscriptionstatus = subscriptionBA.SaveSubscriptionData(actualCriteria);
                    long subscriptionID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(subscriptionstatus)));

                    if (subscriptionID > 0)
                    {
                        // 6. Update the user role as Investor/Broker
                        status = UpdateUserRole(model.RoleID);
                        //Make the user flag as paid
                        SessionController.UserSession.IsPaid = true;

                        //initialize userAuthModel
                        LoginController loginController = new LoginController();
                        var loginBA = new Login();
                        LoginModel loginModel = new LoginModel();
                        HashCriteria.Add("UserName", SessionController.UserSession.EmailAddress);
                        actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                        var rec = loginBA.ValidateLogin(actualCriteria);
                        var loginModelDetails = (LoginModel)(serialization.DeSerializeBinary(Convert.ToString(rec)));

                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);

                        var userSession = loginController.Authenticate(loginModelDetails);
                        if (userSession != null)
                        {
                            var identity = new ClaimsIdentity(AuthenticationHelper.CreateClaim(userSession,
                                                                userSession.UserRole),
                                                                DefaultAuthenticationTypes.ApplicationCookie
                                                                );
                            AuthenticationManager.SignIn(new AuthenticationProperties()
                            {
                                AllowRefresh = true,
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddHours(1)
                            }, identity);
                        }

                        if (model.RoleID == 2)
                        {
                            return RedirectToAction("Investor", "Dashboard");
                        }
                        if (model.RoleID == 3)
                        {
                            return RedirectToAction("Broker", "Dashboard");
                        }
                    }
                }
            }
            return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
        }
        #endregion

        #region Charge Save Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChargeSaveCustomer(SubscriptionModel model)
        {
            var subscriptionBA = new Subscription();
            var serialization = new Serialization();
            var status = false;
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var HashCriteriaPlan = new Hashtable();
            var actualCriteriaPlan = string.Empty;
            var userID = Convert.ToString(SessionController.UserSession.UserId);
            var subscriptionModel = new SubscriptionModel();
            subscriptionModel.CardType = GetPaymentCardType();
            subscriptionModel.ExpirationYearList = GetExpirationYear();
            subscriptionModel.ExpirationMonthList = GetExpirationMonth();
            if (model.RoleID == 2)
            {
                ViewBag.SubscriptionTitle = "Find unlimited investment opportunities for $399 per month.";
            }
            else if (model.RoleID == 3)
            {
                ViewBag.SubscriptionTitle = "List unlimited investment opportunities for $399 per month.";
            }

            //Remove fields form model because these are required fieldsand we are not using these fields on paywall
            ModelState.Remove("State");
            ModelState.Remove("BillingAddress");
            ModelState.Remove("Zip");
            ModelState.Remove("City");
            if (!ModelState.IsValid)
            {
                return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
            }
            //Check if the user is already a custome ron stripe or not?
            var customer_ID = Convert.ToString(SessionController.UserSession.CustomerID);
            if (customer_ID != null && customer_ID != "")
            {
                if (model.Token != null)
                {
                    //For existing customer create new card
                    var cardOptions = new StripeCardCreateOptions()
                    {
                        SourceToken = model.Token
                    };

                    var cardService = new StripeCardService();
                    StripeCard card = cardService.Create(customer_ID, cardOptions);
                }
                else
                {
                    return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
                }
                model.CustomerID = customer_ID;
            }
            else
            {
                // 1. Create customer in stripe
                if (model.Token != null)
                {
                    var customerID = await CreateCustomer(model.Token);
                    model.CustomerID = customerID;
                    SessionController.UserSession.CustomerID = model.CustomerID;
                }
                else
                {
                    return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
                }
            }
            // 2. Get the plans from the Plans table
            HashCriteriaPlan.Add("ID", model.subscriptionOption.ID.ToString());
            actualCriteriaPlan = serialization.SerializeBinary((object)HashCriteriaPlan);

            var result = subscriptionBA.GetPlanDetails(actualCriteriaPlan);
            var subscriptionPlans = (SubscriptionPlans)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var planID = model.subscriptionOption.ID;
            var subscription_PlanID = subscriptionPlans.SubscriptionPlanID;
            var amount = subscriptionPlans.Amount;

            // 3. subscription aginst that plan
            var subscriptionService = new StripeSubscriptionService();
            var stripeSubscription = subscriptionService.Create(model.CustomerID, subscription_PlanID);

            //4. Make the payment
            model.Amount = amount;

            var chargeId = await ProcessPayment(model);

            if (chargeId != null)
            {
                DateTime billingDate = DateTime.Now;
                // 5. Save detals in the subscription table with amount and token of charge
                HashCriteria.Add("Token", model.Token);
                HashCriteria.Add("UserID", userID);
                HashCriteria.Add("Amount", model.Amount);
                HashCriteria.Add("BillingDate", Convert.ToString(billingDate.ToString("dd/MM/yyyy")));
                HashCriteria.Add("CustomerID", model.CustomerID);
                HashCriteria.Add("PlanID", planID);
                HashCriteria.Add("SubscriptionID", stripeSubscription.Id);
                HashCriteria.Add("ChargeID", chargeId);

                actualCriteria = serialization.SerializeBinary((object)HashCriteria);

                var subscriptionstatus = subscriptionBA.SaveSubscriptionData(actualCriteria);
                var subscriptionID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(subscriptionstatus)));

                if (subscriptionID > 0)
                {
                    // 6. Update the user role as Investor/Broker
                    status = UpdateUserRole(model.RoleID);
                    //Make the user flag as paid
                    SessionController.UserSession.IsPaid = true;
                    if (model.RoleID == 2)
                        Synoptek.SessionController.UserSession.RoleType = "Investor";
                    else if (model.RoleID == 3)
                        Synoptek.SessionController.UserSession.RoleType = "Broker";

                    //initialize userAuthModel
                    LoginController loginController = new LoginController();
                    var loginBA = new Login();
                    LoginModel loginModel = new LoginModel();
                    HashCriteria.Add("UserName", SessionController.UserSession.EmailAddress);
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    var rec = loginBA.ValidateLogin(actualCriteria);
                    var loginModelDetails = (LoginModel)(serialization.DeSerializeBinary(Convert.ToString(rec)));

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);

                    var userSession = loginController.Authenticate(loginModelDetails);
                    if (userSession != null)
                    {
                        var identity = new ClaimsIdentity(AuthenticationHelper.CreateClaim(userSession,
                                                            userSession.UserRole),
                                                            DefaultAuthenticationTypes.ApplicationCookie
                                                            );
                        AuthenticationManager.SignIn(new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(1)
                        }, identity);
                    }

                    if (model.RoleID == 2)
                    {
                        return RedirectToAction("Investor", "Dashboard");
                    }
                    if (model.RoleID == 3)
                    {
                        return RedirectToAction("Broker", "Dashboard");
                    }
                }
            }
            return PartialView("_PaymentSubscriptionPopup", subscriptionModel);
        }
        #endregion

        #region  Process the payment
        private static async Task<string> ProcessPayment(SubscriptionModel model)
        {
            var stripeSecretKey = ConfigurationManager.AppSettings["stripeSecretKey"];
            return await Task.Run(() =>
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = Convert.ToInt32(model.Amount),
                    Currency = "usd",
                    Description = "Monthly Subscription Plan",
                    CustomerId = model.CustomerID
                };
                var chargeService = new StripeChargeService(stripeSecretKey);
                var stripeCharge = chargeService.Create(myCharge);
                return stripeCharge.Id;
            });
        }
        #endregion

        #region Create customer in stripe for the user
        private static async Task<string> CreateCustomer(string Token)
        {
            var stripeEmail = Convert.ToString(SessionController.UserSession.EmailAddress);
            var customers = new StripeCustomerService();
            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = Token
            });
            return customer.Id;
        }
        #endregion

        #region Update user role as Investor/Broker
        public bool UpdateUserRole(long roleID)
        {
            var userProfileBA = new UserRegistration();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var userID = SessionController.UserSession.UserId;
            HashCriteria.Add("RoleID", roleID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = userProfileBA.UpdateUserRole(actualCriteria);
            result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
            return true;
        }
        #endregion

        public IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}