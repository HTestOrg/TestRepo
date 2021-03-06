﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessLogic.BrokerListingServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BrokerListingServiceReference.IBrokerListingService")]
    public interface IBrokerListingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/DeleteCurrentListing", ReplyAction="http://tempuri.org/IBrokerListingService/DeleteCurrentListingResponse")]
        string DeleteCurrentListing(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/DeleteCurrentListing", ReplyAction="http://tempuri.org/IBrokerListingService/DeleteCurrentListingResponse")]
        System.Threading.Tasks.Task<string> DeleteCurrentListingAsync(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/GetBrokerListingDetails", ReplyAction="http://tempuri.org/IBrokerListingService/GetBrokerListingDetailsResponse")]
        string GetBrokerListingDetails(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/GetBrokerListingDetails", ReplyAction="http://tempuri.org/IBrokerListingService/GetBrokerListingDetailsResponse")]
        System.Threading.Tasks.Task<string> GetBrokerListingDetailsAsync(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/GetCurrentListingMessages", ReplyAction="http://tempuri.org/IBrokerListingService/GetCurrentListingMessagesResponse")]
        string GetCurrentListingMessages(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/GetCurrentListingMessages", ReplyAction="http://tempuri.org/IBrokerListingService/GetCurrentListingMessagesResponse")]
        System.Threading.Tasks.Task<string> GetCurrentListingMessagesAsync(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/ReplyToMessage", ReplyAction="http://tempuri.org/IBrokerListingService/ReplyToMessageResponse")]
        string ReplyToMessage(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/ReplyToMessage", ReplyAction="http://tempuri.org/IBrokerListingService/ReplyToMessageResponse")]
        System.Threading.Tasks.Task<string> ReplyToMessageAsync(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/ArchiveCurrentMessage", ReplyAction="http://tempuri.org/IBrokerListingService/ArchiveCurrentMessageResponse")]
        string ArchiveCurrentMessage(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/ArchiveCurrentMessage", ReplyAction="http://tempuri.org/IBrokerListingService/ArchiveCurrentMessageResponse")]
        System.Threading.Tasks.Task<string> ArchiveCurrentMessageAsync(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/MarkAsReadMessage", ReplyAction="http://tempuri.org/IBrokerListingService/MarkAsReadMessageResponse")]
        string MarkAsReadMessage(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/MarkAsReadMessage", ReplyAction="http://tempuri.org/IBrokerListingService/MarkAsReadMessageResponse")]
        System.Threading.Tasks.Task<string> MarkAsReadMessageAsync(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/GetMessageListDetails", ReplyAction="http://tempuri.org/IBrokerListingService/GetMessageListDetailsResponse")]
        string GetMessageListDetails(string listingsCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerListingService/GetMessageListDetails", ReplyAction="http://tempuri.org/IBrokerListingService/GetMessageListDetailsResponse")]
        System.Threading.Tasks.Task<string> GetMessageListDetailsAsync(string listingsCriteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBrokerListingServiceChannel : BusinessLogic.BrokerListingServiceReference.IBrokerListingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BrokerListingServiceClient : System.ServiceModel.ClientBase<BusinessLogic.BrokerListingServiceReference.IBrokerListingService>, BusinessLogic.BrokerListingServiceReference.IBrokerListingService {
        
        public BrokerListingServiceClient() {
        }
        
        public BrokerListingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BrokerListingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerListingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerListingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string DeleteCurrentListing(string listingsCriteria) {
            return base.Channel.DeleteCurrentListing(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> DeleteCurrentListingAsync(string listingsCriteria) {
            return base.Channel.DeleteCurrentListingAsync(listingsCriteria);
        }
        
        public string GetBrokerListingDetails(string listingsCriteria) {
            return base.Channel.GetBrokerListingDetails(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetBrokerListingDetailsAsync(string listingsCriteria) {
            return base.Channel.GetBrokerListingDetailsAsync(listingsCriteria);
        }
        
        public string GetCurrentListingMessages(string listingsCriteria) {
            return base.Channel.GetCurrentListingMessages(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetCurrentListingMessagesAsync(string listingsCriteria) {
            return base.Channel.GetCurrentListingMessagesAsync(listingsCriteria);
        }
        
        public string ReplyToMessage(string listingsCriteria) {
            return base.Channel.ReplyToMessage(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> ReplyToMessageAsync(string listingsCriteria) {
            return base.Channel.ReplyToMessageAsync(listingsCriteria);
        }
        
        public string ArchiveCurrentMessage(string listingsCriteria) {
            return base.Channel.ArchiveCurrentMessage(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> ArchiveCurrentMessageAsync(string listingsCriteria) {
            return base.Channel.ArchiveCurrentMessageAsync(listingsCriteria);
        }
        
        public string MarkAsReadMessage(string listingsCriteria) {
            return base.Channel.MarkAsReadMessage(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> MarkAsReadMessageAsync(string listingsCriteria) {
            return base.Channel.MarkAsReadMessageAsync(listingsCriteria);
        }
        
        public string GetMessageListDetails(string listingsCriteria) {
            return base.Channel.GetMessageListDetails(listingsCriteria);
        }
        
        public System.Threading.Tasks.Task<string> GetMessageListDetailsAsync(string listingsCriteria) {
            return base.Channel.GetMessageListDetailsAsync(listingsCriteria);
        }
    }
}
