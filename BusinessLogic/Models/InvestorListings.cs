using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using BusinessLogic.InvestorListingsServiceReference;

namespace BusinessLogic.Models
{
    public class InvestorListings
    {  
        #region Mark as favorite
        public string MarkAsFavorite(string listingCriteria)
        { 
            var investorListingsService = this.GetServiceClient();
            var result = investorListingsService.MarkAsFavorite(listingCriteria);
            return result; 
        }
        #endregion

        #region Send Message To Broker
        public string SendMessageToBroker(string listingCriteria)
        {
            var investorListingsService = this.GetServiceClient();
            var result = investorListingsService.SendMessageToBroker(listingCriteria);
            return result;
        }
        #endregion

        #region Get Broker Email Address
        public string GetBrokerEmailAddress(string listingCriteria)
        {
            var investorListingsService = this.GetServiceClient();
            var result = investorListingsService.GetBrokerEmailAddress(listingCriteria);
            return result;
        }
        #endregion

        #region Get Listing Documents
        public string GetListingDocuments(string listingCriteria)
        {
            var investorListingsService = this.GetServiceClient();
            var result = investorListingsService.GetListingDocuments(listingCriteria);
            return result;
        }
        #endregion

        #region Get Single Listing Details
        public string GetSingleDealDetails(string listingCriteria)
        {
            var investorListingsService = this.GetServiceClient();
            var result = investorListingsService.GetSingleListingDetails(listingCriteria);
            return result;
        }
        #endregion

        #region Get Deal details
        public string GetDealDetails(string listingCriteria)
        {
            var investorListingsService = this.GetServiceClient();
            var investorListings = investorListingsService.GetInvestorListingDetails(listingCriteria);
            return investorListings;
        }
        #endregion

        #region Get User Image
        public string GetUserImage(string criteria)
        {
            var investorListingsService = this.GetServiceClient();
            var userImage = investorListingsService.GetUserImage(criteria);
            return userImage;
        }
        #endregion

        #region Get All Featured List
        public string GetAllFeaturedList()
        {
            var investorListingsService = this.GetServiceClient();
            var investorListings = investorListingsService.GetAllFeaturedList();
            return investorListings;
        }
        #endregion

        #region Get Listing Types
        public List<BusinessObjects.ListingType> GetListingTypes()
        {
            var investorListingsService = this.GetServiceClient();
            return investorListingsService.GetListingTypes();
        }
        #endregion

        #region Get Location List
        public void GetLocationList()
        {
            var investorListingsService = this.GetServiceClient();
            List<BusinessObjects.Location> LocationList = new List<BusinessObjects.Location>();
            LocationList = investorListingsService.GetLocationList();
        }
        #endregion

        #region Get investor service reference
        private IInvestorListingsService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var newsServiceReference = ConfigurationManager.AppSettings["InvestorListing"];
            EndpointAddress endpoint = new EndpointAddress(newsServiceReference);
            IInvestorListingsService client = ChannelFactory<IInvestorListingsService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
