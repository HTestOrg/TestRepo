using BusinessLogic.LearnServiceReference;
using System.Configuration;
using System.ServiceModel;

namespace BusinessLogic.Models
{
    public class Learn
    {
        #region Get Experiences
        public string GetExperienceLevel()
        {
            var learngService = this.GetServiceClient();
            var result = learngService.GetExperienceLevel();
            return result;
        }
        #endregion

        #region Get Topics
        public string GetTopics(string learnCriteria)
        {
            var learnService = this.GetServiceClient();
            var result = learnService.GetTopics(learnCriteria);
            return result;
        }
        #endregion

        #region Get Learn Types
        public string GetLearnTypes()
        {
            var learnService = this.GetServiceClient();
            var result = learnService.GetLearnTypes();
            return result;
        }
        #endregion

        #region Get Deal details
        public string GetLearnListingDetails(string listingCriteria)
        {
            var learnService = this.GetServiceClient();
            var learnListings = learnService.GetLearnListingDetails(listingCriteria);
            return learnListings;
        }
        #endregion

        #region Get Single Learn Webinar Details
        public string GetSingleLearnDetails(string learnCriteria)
        { 
            var learnService = this.GetServiceClient();
            var webinarDetail = learnService.GetSingleLearnDetails(learnCriteria);
            return webinarDetail; 
        }
        #endregion

        #region Delete current listing
        public string DeleteCurrentResource(string learnCriteria)
        {
            var brokerService = this.GetServiceClient();
            var result = brokerService.DeleteCurrentResource(learnCriteria);
            return result;
        }
        #endregion

        #region Edit Current resource
        public string EditCurrentResource(string learnCriteria)
        {
            var learnService = this.GetServiceClient();
            var result = learnService.EditCurrentResource(learnCriteria);
            return result;
        }
        #endregion 

        #region Save Learn Data
        public string SaveLearnData(string learnCriteria)
        {
            var learnService = this.GetServiceClient();
            var result = learnService.SaveLearnData(learnCriteria);
            return result;
        }
        #endregion

        #region Save Uploaded Learn Documents
        public string SaveUploadedLearnDocuments(string learnCriteria)
        {
            var learnService = this.GetServiceClient();
            var result = learnService.SaveUploadedLearnDocuments(learnCriteria);
            return result;
        }
        #endregion

        #region Update Listing Image Path
        public string UpdateLearnImagePath(string learnCriteria)
        {
            var learnService = this.GetServiceClient();
            var result = learnService.InsertUpdateLearnImagePath(learnCriteria);
            return result;
        }
        #endregion

        #region Get Learn Documents
        public string GetLearnDocuments(string learnCriteria)
        {
            var learnService = this.GetServiceClient();
            var result = learnService.GetLearnDocuments(learnCriteria);
            return result;
        }
        #endregion

        #region Get Learn reference
        private ILearnService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var learnServiceReference = ConfigurationManager.AppSettings["LearnService"];
            EndpointAddress endpoint = new EndpointAddress(learnServiceReference);
            ILearnService client = ChannelFactory<ILearnService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
