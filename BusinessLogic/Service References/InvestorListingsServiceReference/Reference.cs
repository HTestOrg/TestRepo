﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessLogic.InvestorListingsServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="InvestorListingsServiceReference.IInvestorListingsService")]
    public interface IInvestorListingsService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetBrokerEmailAddress", ReplyAction="http://tempuri.org/IInvestorListingsService/GetBrokerEmailAddressResponse")]
        string GetBrokerEmailAddress(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetBrokerEmailAddress", ReplyAction="http://tempuri.org/IInvestorListingsService/GetBrokerEmailAddressResponse")]
        System.Threading.Tasks.Task<string> GetBrokerEmailAddressAsync(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetListingDocuments", ReplyAction="http://tempuri.org/IInvestorListingsService/GetListingDocumentsResponse")]
        string GetListingDocuments(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetListingDocuments", ReplyAction="http://tempuri.org/IInvestorListingsService/GetListingDocumentsResponse")]
        System.Threading.Tasks.Task<string> GetListingDocumentsAsync(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetListingTypes", ReplyAction="http://tempuri.org/IInvestorListingsService/GetListingTypesResponse")]
        System.Collections.Generic.List<BusinessObjects.ListingType> GetListingTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetListingTypes", ReplyAction="http://tempuri.org/IInvestorListingsService/GetListingTypesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<BusinessObjects.ListingType>> GetListingTypesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetLocationList", ReplyAction="http://tempuri.org/IInvestorListingsService/GetLocationListResponse")]
        System.Collections.Generic.List<BusinessObjects.Location> GetLocationList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetLocationList", ReplyAction="http://tempuri.org/IInvestorListingsService/GetLocationListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<BusinessObjects.Location>> GetLocationListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetInvestorListingDetails", ReplyAction="http://tempuri.org/IInvestorListingsService/GetInvestorListingDetailsResponse")]
        string GetInvestorListingDetails(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetInvestorListingDetails", ReplyAction="http://tempuri.org/IInvestorListingsService/GetInvestorListingDetailsResponse")]
        System.Threading.Tasks.Task<string> GetInvestorListingDetailsAsync(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetSingleListingDetails", ReplyAction="http://tempuri.org/IInvestorListingsService/GetSingleListingDetailsResponse")]
        string GetSingleListingDetails(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetSingleListingDetails", ReplyAction="http://tempuri.org/IInvestorListingsService/GetSingleListingDetailsResponse")]
        System.Threading.Tasks.Task<string> GetSingleListingDetailsAsync(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/MarkAsFavorite", ReplyAction="http://tempuri.org/IInvestorListingsService/MarkAsFavoriteResponse")]
        string MarkAsFavorite(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/MarkAsFavorite", ReplyAction="http://tempuri.org/IInvestorListingsService/MarkAsFavoriteResponse")]
        System.Threading.Tasks.Task<string> MarkAsFavoriteAsync(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/SendMessageToBroker", ReplyAction="http://tempuri.org/IInvestorListingsService/SendMessageToBrokerResponse")]
        string SendMessageToBroker(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/SendMessageToBroker", ReplyAction="http://tempuri.org/IInvestorListingsService/SendMessageToBrokerResponse")]
        System.Threading.Tasks.Task<string> SendMessageToBrokerAsync(string listingCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetAllFeaturedList", ReplyAction="http://tempuri.org/IInvestorListingsService/GetAllFeaturedListResponse")]
        string GetAllFeaturedList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetAllFeaturedList", ReplyAction="http://tempuri.org/IInvestorListingsService/GetAllFeaturedListResponse")]
        System.Threading.Tasks.Task<string> GetAllFeaturedListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetUserImage", ReplyAction="http://tempuri.org/IInvestorListingsService/GetUserImageResponse")]
        string GetUserImage(string imageCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInvestorListingsService/GetUserImage", ReplyAction="http://tempuri.org/IInvestorListingsService/GetUserImageResponse")]
        System.Threading.Tasks.Task<string> GetUserImageAsync(string imageCriteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IInvestorListingsServiceChannel : BusinessLogic.InvestorListingsServiceReference.IInvestorListingsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InvestorListingsServiceClient : System.ServiceModel.ClientBase<BusinessLogic.InvestorListingsServiceReference.IInvestorListingsService>, BusinessLogic.InvestorListingsServiceReference.IInvestorListingsService {
        
        public InvestorListingsServiceClient() {
        }
        
        public InvestorListingsServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InvestorListingsServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InvestorListingsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InvestorListingsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetBrokerEmailAddress(string listingCriteria) {
            return base.Channel.GetBrokerEmailAddress(listingCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetBrokerEmailAddressAsync(string listingCriteria) {
            return base.Channel.GetBrokerEmailAddressAsync(listingCriteria);
        }
        
        public string GetListingDocuments(string listingCriteria) {
            return base.Channel.GetListingDocuments(listingCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetListingDocumentsAsync(string listingCriteria) {
            return base.Channel.GetListingDocumentsAsync(listingCriteria);
        }
        
        public System.Collections.Generic.List<BusinessObjects.ListingType> GetListingTypes() {
            return base.Channel.GetListingTypes();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<BusinessObjects.ListingType>> GetListingTypesAsync() {
            return base.Channel.GetListingTypesAsync();
        }
        
        public System.Collections.Generic.List<BusinessObjects.Location> GetLocationList() {
            return base.Channel.GetLocationList();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<BusinessObjects.Location>> GetLocationListAsync() {
            return base.Channel.GetLocationListAsync();
        }
        
        public string GetInvestorListingDetails(string listingCriteria) {
            return base.Channel.GetInvestorListingDetails(listingCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetInvestorListingDetailsAsync(string listingCriteria) {
            return base.Channel.GetInvestorListingDetailsAsync(listingCriteria);
        }
        
        public string GetSingleListingDetails(string listingCriteria) {
            return base.Channel.GetSingleListingDetails(listingCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetSingleListingDetailsAsync(string listingCriteria) {
            return base.Channel.GetSingleListingDetailsAsync(listingCriteria);
        }
        
        public string MarkAsFavorite(string listingCriteria) {
            return base.Channel.MarkAsFavorite(listingCriteria);
        }
        
        public System.Threading.Tasks.Task<string> MarkAsFavoriteAsync(string listingCriteria) {
            return base.Channel.MarkAsFavoriteAsync(listingCriteria);
        }
        
        public string SendMessageToBroker(string listingCriteria) {
            return base.Channel.SendMessageToBroker(listingCriteria);
        }
        
        public System.Threading.Tasks.Task<string> SendMessageToBrokerAsync(string listingCriteria) {
            return base.Channel.SendMessageToBrokerAsync(listingCriteria);
        }
        
        public string GetAllFeaturedList() {
            return base.Channel.GetAllFeaturedList();
        }
        
        public System.Threading.Tasks.Task<string> GetAllFeaturedListAsync() {
            return base.Channel.GetAllFeaturedListAsync();
        }
        
        public string GetUserImage(string imageCriteria) {
            return base.Channel.GetUserImage(imageCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetUserImageAsync(string imageCriteria) {
            return base.Channel.GetUserImageAsync(imageCriteria);
        }
    }
}
