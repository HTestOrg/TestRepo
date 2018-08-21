using PagedList;
using System;
using System.Collections.Generic;

namespace BusinessObjects
{
    public class DashboardWrapper
    {
        public List<InvestorDashboardModel> InvestorListing { get; set; }  
        public IPagedList<InvestorDashboardModel> PagedListInvestorActiveListing { get; set; }
        public IPagedList<InvestorDashboardModel> PagedListInvestorFavoriteListing { get; set; }
        public List<BrokerListingsModel> BrokerDashboard { get; set; }  
        public IPagedList<BrokerListingsModel> PagedListBrokerDashboard { get; set; }   
        public IPagedList<UserProfileEditModel> PagedListUsers { get; set; }
        public IPagedList<LearnModel> PagedListResource { get; set; }
        public IPagedList<TestimonialModel> PagedListTestimonial { get; set; } 
        public IPagedList<InvestorListingsModel> PagedListAdminListings {get; set;} 
        public List<InvestorListingsModel> InvestorAdminListing { get; set; }
        public List<UserProfileEditModel> Users { get; set; }
        public List<LearnModel> Resources { get; set; }
        public List<TestimonialModel> Testimonials { get; set; } 
    }

    [Serializable()]
    public class InvestorDashboardModel
    {
        public long ID { get; set; }
        public string Name { get; set; } 
        public string ListingType { get; set; }
        public double AmountRequired { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; } 
        public long BrokerId { get; set; } 
        public bool IsFavorite { get; set; } 
        public int MessageCount { get; set; }
        public string Status { get; set; }
        public bool IsSponsored { get; set; }
        public int SentMessageCount { get; set; }
    }
}
