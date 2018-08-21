using System.Collections.Generic;

namespace BusinessObjects
{
    public class HomePageModel
    {
        public List<HomePageLatestResources> resourceList { get; set; } 
        public List<TestimonialModel> TestimonialList { get; set; } 
        public List<BrokerListingsModel> brokerList { get; set; } 
        public LoginModel LoginModel { get; set; } 
        public UserProfileModel UserProfileModel { get; set; } 
        public List<InvestorListingsModel> FeaturedList { get; set; }
    }
    public class HomePageLatestResources
    {
        public int ID { get; set; } 
        public string Title { get; set; } 
        public string Category { get; set; } 
        public string ImagePath { get; set; } 
        public int ContentType { get; set; } 
        public long TopicID { get; set; }
        public int ContentFor { get; set; }
        public string ContentName { get; set; }
        public string LearnCategory { get; set; }
    }
}