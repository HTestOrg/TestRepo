using System.Collections.Generic;
using PagedList;
using System;

namespace BusinessObjects
{
    [Serializable()]
    public class BrokerListingsModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ListingType { get; set; }
        public double AmountRequired { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }
        public string ListingStatus { get; set; }          // Active, Inactive and Draft
        public bool IsSponsored { get; set; }
        public int MessageCount { get; set; }
    }

    [Serializable()]
    public class BrokerListingsWrapper
    {
        public List<BrokerListingsModel> Active { get; set; }
        public List<BrokerListingsModel> Drafts { get; set; }
        public List<BrokerListingsModel> Inactive { get; set; }
        public IPagedList<BrokerListingsModel> PagedListActive { get; set; }
        public IPagedList<BrokerListingsModel> PagedListDrafts { get; set; }
        public IPagedList<BrokerListingsModel> PagedListInactive { get; set; }

    }

    //[Serializable()]
    //public class ListingCommentsWrapper
    //{
    //    public List<ListingComments> ListingComments { get; set; } 
    //}

    [Serializable()]
    public class ListingComments
    {
        public long CommentID { get; set; }
        public long ListingId { get; set; }
        public string ListingName { get; set; }
        public string ImagePath { get; set; }
        public string MessageFrom { get; set; } 
        public string Duration { get; set; }
        public string MessageTimeStamp { get; set; }
        public string MessageText { get; set; }
        public long FromID { get; set; }
        public int MessageCount { get; set; }  
        public List<ListingComments> RepliedComments { get; set; }
    }

    [Serializable()]
    public class BrokerDetailsWrapper
    {
        public List<BrokerListingsModel> ListingDetails { get; set; }
        public IPagedList<BrokerListingsModel> PagedListingDetails { get; set; }
        public string BrokerName { get; set; }
        public string BrokerImage { get; set; }
        public long BrokerID { get; set; }  
    }
}