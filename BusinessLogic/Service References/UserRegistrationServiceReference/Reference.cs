﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessLogic.UserRegistrationServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserRegistrationServiceReference.IUserRegistrationService")]
    public interface IUserRegistrationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/SaveUserProfile", ReplyAction="http://tempuri.org/IUserRegistrationService/SaveUserProfileResponse")]
        string SaveUserProfile(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/SaveUserProfile", ReplyAction="http://tempuri.org/IUserRegistrationService/SaveUserProfileResponse")]
        System.Threading.Tasks.Task<string> SaveUserProfileAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/UpdateUserProfile", ReplyAction="http://tempuri.org/IUserRegistrationService/UpdateUserProfileResponse")]
        string UpdateUserProfile(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/UpdateUserProfile", ReplyAction="http://tempuri.org/IUserRegistrationService/UpdateUserProfileResponse")]
        System.Threading.Tasks.Task<string> UpdateUserProfileAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/GetUserSpecificDetails", ReplyAction="http://tempuri.org/IUserRegistrationService/GetUserSpecificDetailsResponse")]
        string GetUserSpecificDetails(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/GetUserSpecificDetails", ReplyAction="http://tempuri.org/IUserRegistrationService/GetUserSpecificDetailsResponse")]
        System.Threading.Tasks.Task<string> GetUserSpecificDetailsAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/GetSecurityQuestionList", ReplyAction="http://tempuri.org/IUserRegistrationService/GetSecurityQuestionListResponse")]
        System.Collections.Generic.List<BusinessObjects.SecurityQuestion> GetSecurityQuestionList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/GetSecurityQuestionList", ReplyAction="http://tempuri.org/IUserRegistrationService/GetSecurityQuestionListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<BusinessObjects.SecurityQuestion>> GetSecurityQuestionListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/CheckIfUserIsAlredayExistOrNot", ReplyAction="http://tempuri.org/IUserRegistrationService/CheckIfUserIsAlredayExistOrNotRespons" +
            "e")]
        string CheckIfUserIsAlredayExistOrNot(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/CheckIfUserIsAlredayExistOrNot", ReplyAction="http://tempuri.org/IUserRegistrationService/CheckIfUserIsAlredayExistOrNotRespons" +
            "e")]
        System.Threading.Tasks.Task<string> CheckIfUserIsAlredayExistOrNotAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/UpdateProfileImagePath", ReplyAction="http://tempuri.org/IUserRegistrationService/UpdateProfileImagePathResponse")]
        string UpdateProfileImagePath(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/UpdateProfileImagePath", ReplyAction="http://tempuri.org/IUserRegistrationService/UpdateProfileImagePathResponse")]
        System.Threading.Tasks.Task<string> UpdateProfileImagePathAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/CheckIfUserEnterCorrectPassword", ReplyAction="http://tempuri.org/IUserRegistrationService/CheckIfUserEnterCorrectPasswordRespon" +
            "se")]
        string CheckIfUserEnterCorrectPassword(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/CheckIfUserEnterCorrectPassword", ReplyAction="http://tempuri.org/IUserRegistrationService/CheckIfUserEnterCorrectPasswordRespon" +
            "se")]
        System.Threading.Tasks.Task<string> CheckIfUserEnterCorrectPasswordAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/UpdateUserRole", ReplyAction="http://tempuri.org/IUserRegistrationService/UpdateUserRoleResponse")]
        string UpdateUserRole(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/UpdateUserRole", ReplyAction="http://tempuri.org/IUserRegistrationService/UpdateUserRoleResponse")]
        System.Threading.Tasks.Task<string> UpdateUserRoleAsync(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/DeleteUser", ReplyAction="http://tempuri.org/IUserRegistrationService/DeleteUserResponse")]
        string DeleteUser(string userProfileCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserRegistrationService/DeleteUser", ReplyAction="http://tempuri.org/IUserRegistrationService/DeleteUserResponse")]
        System.Threading.Tasks.Task<string> DeleteUserAsync(string userProfileCriteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserRegistrationServiceChannel : BusinessLogic.UserRegistrationServiceReference.IUserRegistrationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserRegistrationServiceClient : System.ServiceModel.ClientBase<BusinessLogic.UserRegistrationServiceReference.IUserRegistrationService>, BusinessLogic.UserRegistrationServiceReference.IUserRegistrationService {
        
        public UserRegistrationServiceClient() {
        }
        
        public UserRegistrationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserRegistrationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserRegistrationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserRegistrationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string SaveUserProfile(string userProfileCriteria) {
            return base.Channel.SaveUserProfile(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> SaveUserProfileAsync(string userProfileCriteria) {
            return base.Channel.SaveUserProfileAsync(userProfileCriteria);
        }
        
        public string UpdateUserProfile(string userProfileCriteria) {
            return base.Channel.UpdateUserProfile(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> UpdateUserProfileAsync(string userProfileCriteria) {
            return base.Channel.UpdateUserProfileAsync(userProfileCriteria);
        }
        
        public string GetUserSpecificDetails(string userProfileCriteria) {
            return base.Channel.GetUserSpecificDetails(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetUserSpecificDetailsAsync(string userProfileCriteria) {
            return base.Channel.GetUserSpecificDetailsAsync(userProfileCriteria);
        }
        
        public System.Collections.Generic.List<BusinessObjects.SecurityQuestion> GetSecurityQuestionList() {
            return base.Channel.GetSecurityQuestionList();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<BusinessObjects.SecurityQuestion>> GetSecurityQuestionListAsync() {
            return base.Channel.GetSecurityQuestionListAsync();
        }
        
        public string CheckIfUserIsAlredayExistOrNot(string userProfileCriteria) {
            return base.Channel.CheckIfUserIsAlredayExistOrNot(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> CheckIfUserIsAlredayExistOrNotAsync(string userProfileCriteria) {
            return base.Channel.CheckIfUserIsAlredayExistOrNotAsync(userProfileCriteria);
        }
        
        public string UpdateProfileImagePath(string userProfileCriteria) {
            return base.Channel.UpdateProfileImagePath(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> UpdateProfileImagePathAsync(string userProfileCriteria) {
            return base.Channel.UpdateProfileImagePathAsync(userProfileCriteria);
        }
        
        public string CheckIfUserEnterCorrectPassword(string userProfileCriteria) {
            return base.Channel.CheckIfUserEnterCorrectPassword(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> CheckIfUserEnterCorrectPasswordAsync(string userProfileCriteria) {
            return base.Channel.CheckIfUserEnterCorrectPasswordAsync(userProfileCriteria);
        }
        
        public string UpdateUserRole(string userProfileCriteria) {
            return base.Channel.UpdateUserRole(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> UpdateUserRoleAsync(string userProfileCriteria) {
            return base.Channel.UpdateUserRoleAsync(userProfileCriteria);
        }
        
        public string DeleteUser(string userProfileCriteria) {
            return base.Channel.DeleteUser(userProfileCriteria);
        }
        
        public System.Threading.Tasks.Task<string> DeleteUserAsync(string userProfileCriteria) {
            return base.Channel.DeleteUserAsync(userProfileCriteria);
        }
    }
}
