using BusinessLogic.LoginServiceReference;
using System.ServiceModel;
using System.Configuration;

namespace BusinessLogic.Models
{
    public class Login
    {
        #region Validate Login details
        public string ValidateLogin(string loginCriteria)
        {
            var loginService = this.GetServiceClient();
            var result = loginService.LoginUser(loginCriteria);
            return result;
        }
        #endregion

        #region Change Password
        public string ChangePassword(string loginCriteria)
        {
            var loginService = this.GetServiceClient();
            var result = loginService.ChangePassword(loginCriteria);
            return result;
        }
        #endregion

        #region Get User Details
        public string GetUserDetails(string loginCriteria)
        {
            var loginService = this.GetServiceClient();
            var result = loginService.GetUserDetails(loginCriteria);
            return result;
        }
        #endregion

        #region Validation Security Questions
        public string ValidationSecQuestions(string loginCriteria)
        {
            var loginService = this.GetServiceClient();
            var result = loginService.ValidationSecQuestions(loginCriteria);
            return result;
        }
        #endregion

        #region Get login service reference
        private ILoginService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var newsServiceReference = ConfigurationManager.AppSettings["LoginService"];
            EndpointAddress endpoint = new EndpointAddress(newsServiceReference);
            ILoginService client = ChannelFactory<ILoginService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
