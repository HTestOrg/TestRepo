using System;
using System.Collections.Generic;
using BusinessObjects;
using BusinessLogic.UserRegistrationServiceReference;
using System.ServiceModel;
using System.Configuration;

namespace BusinessLogic.Models
{
    public class UserRegistration
    {
        #region Get Security Question list
        public List<SecurityQuestion> GetSecurityQuestionList()
        {
            var userRegistrationService = this.GetServiceClient();
            List<SecurityQuestion> lstSecurityQuestion = userRegistrationService.GetSecurityQuestionList();
            return lstSecurityQuestion;
        }
        #endregion

        #region Save User Profile
        public string SaveUserProfile(string userProfileCriteria)
        {
            var userRegistrationService = this.GetServiceClient();
            var result = userRegistrationService.SaveUserProfile(userProfileCriteria);
            return result;
        }
        #endregion

        #region Update User Profile
        public string UpdateUserProfile(string userProfileCriteria)
        {
            var userRegistrationService = this.GetServiceClient();
            var result = userRegistrationService.UpdateUserProfile(userProfileCriteria);
            return result;
        }
        #endregion

        #region Get User Profile
        public string GetUserSpecificDetails(string userProfileCriteria)
        {
            var userRegistrationService = this.GetServiceClient();
            var result = userRegistrationService.GetUserSpecificDetails(userProfileCriteria);
            return result;
        }
        #endregion

        #region Check if user is already exist
        public string CheckForUserName(string userProfileCriteria)
        {
            var result = String.Empty;
            var userRegistrationService = this.GetServiceClient();
            if (!string.IsNullOrWhiteSpace(userProfileCriteria))
            {
                result = userRegistrationService.CheckIfUserIsAlredayExistOrNot(userProfileCriteria);
            }
            return result;
        }
        #endregion

        #region Check If User Enter the Correct Password or not
        public string CheckIfUserEnterCorrectPassword(string userProfileCriteria)
        {
            var result = String.Empty;
            var userRegistrationService = this.GetServiceClient();
            if (!string.IsNullOrWhiteSpace(userProfileCriteria))
            {
                result = userRegistrationService.CheckIfUserEnterCorrectPassword(userProfileCriteria);
            }
            return result;
        }
        #endregion

        #region Update Profile Image Path
        public string UpdateProfileImagePath(string userProfileCriteria)
        {
            var userRegistrationService = this.GetServiceClient();
            var result = userRegistrationService.UpdateProfileImagePath(userProfileCriteria);
            return result;
        }
        #endregion

        #region Update User Role as Investor/Broker
        public string UpdateUserRole(string userProfileCriteria)
        {
            var userRegistrationService = this.GetServiceClient();
            var result = userRegistrationService.UpdateUserRole(userProfileCriteria);
            return result;
        }
        #endregion

        #region Get User Registration Service reference
        private IUserRegistrationService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var userRegistrationServiceReference = ConfigurationManager.AppSettings["UserRegistrationService"];
            EndpointAddress endpoint = new EndpointAddress(userRegistrationServiceReference);
            IUserRegistrationService client = ChannelFactory<IUserRegistrationService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion

        #region Delete User
        public string DeleteUser(string userProfileCriteria)
        {
            var userRegistrationService = this.GetServiceClient();
            var result = userRegistrationService.DeleteUser(userProfileCriteria);
            return result;
        }
        #endregion
    }
}
