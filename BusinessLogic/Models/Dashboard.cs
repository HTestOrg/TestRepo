using BusinessLogic.DashboardServiceReference;
using System.Configuration;
using System.ServiceModel;

namespace BusinessLogic.Models
{
    public class Dashboard
    {
        #region Get Investor Dashboard
        public string GetInvestorDashboard(string actualCriteria)
        {
            var investorDashboardService = this.GetServiceClient();
            var investorDashboard = investorDashboardService.GetInvestorDashboard(actualCriteria);
            return investorDashboard;
        }
        #endregion

        #region Get investor service reference
        private IDashboardService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var dashboardReference = ConfigurationManager.AppSettings["Dashboard"];
            EndpointAddress endpoint = new EndpointAddress(dashboardReference);
            IDashboardService client = ChannelFactory<IDashboardService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
