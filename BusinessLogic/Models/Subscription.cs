using BusinessLogic.SubscriptionServiceReference;
using BusinessObjects;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;

namespace BusinessLogic.Models
{
    public class Subscription
    {
        #region Get Payment Card Types
        public List<PaymentCardTypes> GetPaymentCardTypeDetails()
        {
            var subscriptionService = this.GetServiceClient();
            List<PaymentCardTypes> lstCardTypes = new List<PaymentCardTypes>();
            lstCardTypes = subscriptionService.GetPaymentCardTypeDetails();
            return lstCardTypes;
        }
        #endregion

        #region Save Subscription Data
        public string SaveSubscriptionData(string subscriptionCriteria)
        {
            var subscriptionService = this.GetServiceClient();
            var result = subscriptionService.SaveSubscriptionData(subscriptionCriteria);
            return result;
        }
        #endregion

        #region Get Plan Details
        public string GetPlanDetails(string subscriptionCriteria)
        {
            var subscriptionService = this.GetServiceClient();
            var result = subscriptionService.GetPlanDetails(subscriptionCriteria);
            return result;
        }
        #endregion

        #region Get Subscription Options for User
        public List<SubscriptionOption> GetSubscriptionOptionsList(string subscriptionOptionCriteria)
        {
            var subscriptionService = this.GetServiceClient();
            List<SubscriptionOption> listSubscriptionOption = new List<SubscriptionOption>();
            listSubscriptionOption = subscriptionService.GetSubscriptionOptions(subscriptionOptionCriteria);
            return listSubscriptionOption;
        }
        #endregion

        #region Get Subscription Services
        private ISubscriptionService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var subscriptionServiceReference = ConfigurationManager.AppSettings["SubscriptionService"];
            EndpointAddress endpoint = new EndpointAddress(subscriptionServiceReference);
            ISubscriptionService client = ChannelFactory<ISubscriptionService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
