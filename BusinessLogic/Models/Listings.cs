using BusinessObjects;
using System.Collections.Generic;
using System.ServiceModel;
using BusinessLogic.ListingServiceReference;
using System.Configuration;

namespace BusinessLogic.Models
{
    public class Listings
    {
        #region Edit Current Listing
        public string EditCurrentListing(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.EditCurrentListing(listingCriteria);
            return result;
        }
        #endregion  

        #region Get Loan Images
        public string GetLoanImages(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.GetLoanImages(listingCriteria);
            return result;
        }
        #endregion

        #region Get Loan Documents
        public string GetLoanDocuments(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.GetLoanDocuments(listingCriteria);
            return result;
        }
        #endregion

        #region Update Listing Image Path
        public string UpdateListingImagePath(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.InsertUpdateListingImagePath(listingCriteria);
            return result;
        }
        #endregion

        #region Get Listing Loan Types
        public List<ListingType> GetListingLoanTypes()
        {
            var listingService = this.GetServiceClient();
            List<ListingType> lstListings = new List<ListingType>();
            lstListings = listingService.GetListingLoanTypes();
            return lstListings;
        }
        #endregion

        #region Get Listing Loan Types For Loan Information
        public string GetListingDefaultValuesForLoanInformation()
        {
            var listingService = this.GetServiceClient();
            ListingModel lstListings = new ListingModel();
            var result = listingService.GetListingDefaultValuesForLoanInformation();
            return result;
        }
        #endregion

        #region Get Listing Loan Types For Property Information
        public string GetListingDefaultValuesForPropertyInformation()
        {
            var listingService = this.GetServiceClient();
            ListingModel lstListings = new ListingModel();
            var result = listingService.GetListingDefaultValuesForPropertyInformation();
            return result;
        }
        #endregion

        #region Save Loan Information
        public string SaveLoanInformation(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.SaveLoanInformation(listingCriteria);
            return result;
        }
        #endregion

        #region Save Property Information
        public string SavePropertyInformation(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.SavePropertyInformation(listingCriteria);
            return result;
        }
        #endregion

        #region Save Comments Information
        public string SaveCommentsInformation(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.SaveCommentsInformation(listingCriteria);
            return result;
        }
        #endregion

        #region Save Documents Information
        public string SaveDocumentsInformation(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.SaveDocumentsInformation(listingCriteria);
            return result;
        }
        #endregion

        #region SaveUploadedListingDocuments
        public string SaveUploadedListingDocuments(string listingCriteria)
        {
            var listingService = this.GetServiceClient();
            var result = listingService.SaveUploadedListingDocuments(listingCriteria);
            return result;
        }
        #endregion

        #region Get news reference
        private IListingService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var newsServiceReference = ConfigurationManager.AppSettings["ListingService"];
            EndpointAddress endpoint = new EndpointAddress(newsServiceReference);
            IListingService client = ChannelFactory<IListingService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
