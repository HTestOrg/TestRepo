using BusinessLogic.BrokerListingServiceReference;
using System.ServiceModel;
using System.Configuration;

namespace BusinessLogic.Models
{
    public class BrokerListings
    {
        #region Delete current listing
        public string DeleteCurrentListing(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.DeleteCurrentListing(listingsCriteria);
            return result;
        }
        #endregion

        #region Get Current Listing Messages
        public string GetCurrentListingMessages(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.GetCurrentListingMessages(listingsCriteria);
            return result;
        }
        #endregion

        #region Get Posting Details
        public string GetPostingDetails(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.GetBrokerListingDetails(listingsCriteria);
            return result;
        }
        #endregion

        #region Reply To Message
        public string ReplyToMessage(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.ReplyToMessage(listingsCriteria);
            return result;
        }
        #endregion



        #region Archive Current Message
        public string ArchiveCurrentMessage(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.ArchiveCurrentMessage(listingsCriteria);
            return result;
        }
        #endregion

        //#region Delete Current Message
        //public string DeleteCurrentMessage(string listingsCriteria)
        //{
        //    var brokerService = this.GetServiceClient();
        //    var result = brokerService.DeleteCurrentMessage(listingsCriteria);
        //    return result;
        //}
        //#endregion

        #region Mark Message As Read
        public string MarkAsReadMessage(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.MarkAsReadMessage(listingsCriteria);
            return result;
        }
        #endregion

        #region Get User Messages
        public string GetUserMessage(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.GetMessageListDetails(listingsCriteria);
            return result;
        }
        #endregion

        #region Get Unread Messages
        public string GetUnreadMessages(string listingsCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.GetMessageListDetails(listingsCriteria);
            return result;
        }
        #endregion

        #region Get Broker Listing service reference
        private IBrokerListingService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var BrokerListingServiceReference = ConfigurationManager.AppSettings["BrokerListingService"];
            EndpointAddress endpoint = new EndpointAddress(BrokerListingServiceReference);
            IBrokerListingService client = ChannelFactory<IBrokerListingService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
