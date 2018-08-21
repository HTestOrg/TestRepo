using BusinessObjects;
using System.Collections.Generic;
using BusinessLogic.HomePageServiceReference;
using System.ServiceModel;
using System.Configuration;

namespace BusinessLogic.Models
{
    public class HomePage
    {
        #region Get broker listing details for home page
        public List<BrokerListingsModel> GetListingDetailsForHomePage()
        {
            var homepageService = this.GetServiceClient();
            List<BrokerListingsModel> lstBrokerListingsModel = homepageService.GetLatestBrokerListingDetails();
            return lstBrokerListingsModel;
        }
        #endregion

        #region Get Latest News and Articles For Home Page
        public List<HomePageLatestResources> GetLatestNewsandArticlesForHomePage()
        {
            var homepageService = this.GetServiceClient();
            List<HomePageLatestResources> lstHomePageLatestResources = homepageService.GetLatestNewsandArticlesForHomePage();
            return lstHomePageLatestResources;
        }
        #endregion

        #region Get Home Page service reference
        private IHomePageService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var newsServiceReference = ConfigurationManager.AppSettings["HomePageService"];
            EndpointAddress endpoint = new EndpointAddress(newsServiceReference);
            IHomePageService client = ChannelFactory<IHomePageService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}