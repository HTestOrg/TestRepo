using System.Collections.Generic;
using PagedList;
using System;
using System.ComponentModel;

namespace BusinessObjects
{
    [Serializable()]
    public class InvestorListingsModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ListingType { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }
        public bool IsSponsored { get; set; }
        public long BrokerId { get; set; }
        public string BrokerName { get; set; }
        public bool IsFavorite { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirmName { get; set; }
        public List<ListingImage> ImageList { get; set; }
        public List<ListingLoanDocuments> listingDocuments { get; set; }
        public string LienPositionName { get; set; }
        public string NoteInterestRate { get; set; }
        public string PrincipalBalance { get; set; }
        public string SellerOfferingPercentage { get; set; }
        public string ActualLoanStatus { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public double AmountRequired { get; set; }

        public NoteGeneralInformation noteGeneralInfo { get; set; }
        public NoteTermsTab noteTermsTab { get; set; }
        public NoteDatesTab noteDatesTab { get; set; }
        public ForeClosureTab foreClosureTab { get; set; }
        public PropertyTab propertyTab { get; set; }
    }

    [Serializable()]
    public class Location
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    [Serializable()]
    public class InvestorListingWrapper
    {
        public IPagedList<InvestorListingsModel> PagedListInvestorListings { get; set; }
        public IPagedList<InvestorListingsModel> PagedListFeaturedListings { get; set; }

        public List<InvestorListingsModel> InvestorListings { get; set; }
        public List<InvestorListingsModel> FeaturedListings { get; set; }

        public string ListingTypeIds { get; set; }
        public List<ListingType> ListingTypeList { get; set; }

        public string LocationName { get; set; }
        public string SortingOrder { get; set; }

        public List<ListingType> ListingType { get; set; }
        public int ListingTypeID { get; set; }
        public List<CommonList> PropertyType { get; set; }
        public int PropertyTypeID { get; set; }
        public List<CommonList> LienPosition { get; set; }
        public int LienPositionID { get; set; }
        public int LoanID { get; set; }
        public int LoanStatusID { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int LoanTypeID { get; set; }
        public string PricipalBalance { get; set; }
        public string InterestRate { get; set; }
        public string AskingPrice { get; set; }
        public int ActualLoanStatusID { get; set; }
        public long BrokerId { get; set; }
    }

    [Serializable()]
    public class SingleListingWrapper
    {
        public InvestorListingsModel SingleInvestorListings { get; set; }
        public List<InvestorListingsModel> FeaturedOpportunityList { get; set; }
    }

    [Serializable()]
    public class NoteGeneralInformation
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string LienPositionName { get; set; }
        public string ActualLoanStatus { get; set; }        
        public string LoanType { get; set; }
        [DisplayName("# Payments Last 12 Months")]
        public string NumberOfPaymentsLast12Months { get; set; }
        [DisplayName("Paid To")]
        public string PaidToDate { get; set; } 
        public string LoanMaturityDate { get; set; }
        public string PropertyType { get; set; }
        public string OriginalBalance { get; set; }
        public string PrincipalBalance { get; set; }
        public string EstMarketValue { get; set; }
        public string PaymentAmount { get; set; }
        public string NoteInterestRate { get; set; }
        public string SoldInterestRate { get; set; }
    }

    [Serializable()]
    public class NoteTermsTab
    {
        public string LienPositionName { get; set; }
        public string AmortizationType { get; set; }
        public string LoanType { get; set; }
        public string OriginalBalance { get; set; }
        public string CurrentBalance { get; set; }
        public string PAndL { get; set; }
        public string TotalPayment { get; set; }
        public string NoteInterestRate { get; set; }
        public string SoldInterestRate { get; set; }
        public string TotalInTrust { get; set; }
        public string UnpaidInterest { get; set; }
        public string LateChargesIs { get; set; }
        public string UnpaidCharges { get; set; }
        public string PrepaymentPenalty { get; set; }
        public string RateType { get; set; }
        public string BalloonPayment { get; set; }
        public string LoanTermsModified { get; set; }
        public string Registered_wMERS { get; set; }
        public string OnForbearancePlan { get; set; }
        public string InForeclosure { get; set; }       
        public string InBankruptcy { get; set; }
        public string SellerComments { get; set; }
    }

    [Serializable()]
    public class NoteDatesTab
    {
        public string OriginationDate { get; set; }
        public string FirstPaymentDate { get; set; }
        public string PaidToDate { get; set; }
        public string NextDueDate { get; set; }
        public string MaturityDate { get; set; }
        public string LastPaymentReceived { get; set; }
    }

    [Serializable()]
    public class ForeClosureTab
    {
        public string DateOpened { get; set; }
        public string ScheduleSaleDate { get; set; }
    }

    [Serializable()]
    public class PropertyTab
    {
        public string PropertyType { get; set; }
        public string PropertyAddress { get; set; }
        public string City { get; set; }
        public string MarketValue { get; set; }
        public string LTVRatio { get; set; }
        public string Country { get; set; }
        public string ValuationDate { get; set; }
    }
}