using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    [Serializable()]
    public class ListingModel
    {
        public long ID { get; set; }
        public LoanInformation loanInformation { get; set; }
        public PropertyInformation propertyInformation { get; set; }
        public CommentsInformation commentsInformation { get; set; }
        public Documents documents { get; set; }
    }

    [Serializable()]
    public class LoanInformation
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [DisplayName("Listing Title")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Loan Type field is required.")]
        [DisplayName("Loan Type")]
        public int ListingTypeID { get; set; }
        public List<ListingType> ListingType { get; set; }

        [DisplayName("Rate Type")]
        [Required(ErrorMessage = "The Rate Type field is required.")]
        public int RateTypeID { get; set; }
        public List<CommonList> RateType { get; set; }

        [DisplayName("Amortization Type")]
        [Required(ErrorMessage = "The Amortization Type field is required.")]
        public int AmortizationTypeID { get; set; }
        public List<CommonList> AmortizationType { get; set; }

        [DisplayName("Pre-Pay Penalty")]
        public bool PrePayPenalty { get; set; }

        [DisplayName("Balloon Payment")]
        public bool BalloonPayment { get; set; }

        [DisplayName("Loan Terms Modified")]
        public bool LoanTermsModified { get; set; }

        [DisplayName("In Default")]
        public bool InDefault { get; set; }

        [DisplayName("Registered w/MERS")]
        public bool Registered_wMERS { get; set; }

        [DisplayName("On Forbearance")]
        public bool OnForbearance { get; set; }

        [Display(Name = "In Foreclosure")]
        [Required(ErrorMessage = "The In Foreclosure field is required.")]
        public string InForeclosureID { get; set; }

        [Display(Name = "In Bankruptcy")]
        [Required(ErrorMessage = "The In Bankruptcy field is required.")]
        public string InBankruptcyID { get; set; }

        [Display(Name = "Origination Date")]
        [Required(ErrorMessage = "The Origination Date field is required.")]
        public string OriginationDate { get; set; }

        [Display(Name = "Paid to Date")]
        [Required(ErrorMessage = "The Paid to Date field is required.")]
        public string PaidToDate { get; set; }

        [Display(Name = "Next Payment Date")]
        [Required(ErrorMessage = "The Next Payment Date field is required.")]
        public string NextPaymentDate { get; set; }

        [Display(Name = "Payer's Last Payment Made Date")]
        public string PayersLastPaymentMadeDate { get; set; }

        [Display(Name = "Original Loan Amount $")]
        [Required(ErrorMessage = "The Original Loan Amount $ field is required.")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Original Loan Amount is not valid.")]
        public double OriginalLoanAmount { get; set; }

        [Display(Name = "Unpaid Principal Balance")]
        [Required(ErrorMessage = "The Unpaid Principal Balance field is required.")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Unpaid Principal Balance is not valid.")]
        public double UnpaidPrincipalBalance { get; set; }

        [Display(Name = "Note Maturity Date")]
        [Required(ErrorMessage = "The Note Maturity Date field is required.")]
        public string NoteMaturityDate { get; set; }

        [Display(Name = "Accrued Late Charges")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Accrued Late Charges is not valid.")]
        public double AccruedLateCharges { get; set; }

        [Display(Name = "Loan Charges")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Loan Charges is not valid.")]
        public double LoanCharges { get; set; }

        [Display(Name = "# of Payments Last 12")]
        public int NoOfPaymentsInLast12 { get; set; }

        [Display(Name = "First Payment Date")]
        public string FirstPaymentDate { get; set; }

        [Display(Name = "Escrow/Impounds")]
        public string EscrowImpounds { get; set; }

        [Display(Name = "Note Interest Rate")]
        [Required(ErrorMessage = "The Note Interest Rate field is required.")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Note Interest Rate is not valid.")]
        public double NoteInterestRate { get; set; }


        [Display(Name = "Sold Interest Rate")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Sold Interest Rate is not valid.")]
        public double SoldInterestRate { get; set; }

        [Required(ErrorMessage = "The Payments Frequency field is required.")]
        [DisplayName("Payments Frequency")]
        public int PaymentsFrequencyID { get; set; }
        public List<CommonList> PaymentsFrequency { get; set; } // fetch it from DB

        [Required(ErrorMessage = "The Late charge is field is required.")]
        [DisplayName("Late charge is")]
        public double LateCharges { get; set; }

        [DisplayName(".")]
        public int LateChargesType { get; set; }

        [Required(ErrorMessage = "The Late Charges After XDays field is required.")]
        [DisplayName("After (Day's)")]
        public int LateChargesAfterXDays { get; set; }

        [DisplayName("Unpaid Interest")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Unpaid Interest is not valid.")]
        public double UnpaidInterest { get; set; }

        [DisplayName("Property Taxes Due")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Property Taxes Due is not valid.")]
        public double PropertyTaxesDue { get; set; }

        [DisplayName("Payment Trust")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Payment Trust is not valid.")]
        public double PaymentTrust { get; set; }


        [DisplayName("P&I")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "P&I is not valid.")]
        public double pAndL { get; set; }

        [Required(ErrorMessage = "The Total Monthly Loan Payment field is required.")]
        [DisplayName("Total Monthly Loan Payment")]
        public double TotalMonthlyLoanPayment { get; set; }

        [Required(ErrorMessage = "The Check all that apply field is required.")]
        [DisplayName("Check all that apply")]
        public int CheckAllThatApplyID { get; set; }
        public List<CommonList> CheckAllThatApply { get; set; }            // fetch it from DB

        [Required(ErrorMessage = "The If Junior Trust Deed/Mortgage, lien is field is required.")]
        [DisplayName("If Junior Trust Deed/Mortgage, lien is")]
        public int JuniorTrustDeedORMortgageID { get; set; }
        public List<CommonList> JuniorTrustDeedORMortgageList { get; set; }            // fetch it from DB

        [Required(ErrorMessage = "The Lien Position field is required.")]
        [DisplayName("Lien Position")]
        public int LienPositionID { get; set; }
        public List<CommonList> LienPositionType { get; set; }

        [DisplayName("Principal Balance")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Principal Balance is not valid.")]
        public double PrincipalBalance { get; set; }

        [DisplayName("Monthly Payment")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Monthly Payment is not valid.")]
        public double MonthlyPayment { get; set; }

        [Display(Name = "Modified")]
        public string Modified { get; set; }

        [Display(Name = "Current")]
        public string Current { get; set; }

        [DisplayName("Information as of")]
        public string InformationAsOf { get; set; }

        [DisplayName("Draw Period Start Date")]
        public string DrawPeriodStartDate { get; set; }

        [DisplayName("Repayment Period Start Date")]
        public string RepaymentPeriodStartDate { get; set; }

        [DisplayName("Next Adjustment")]
        public string ArmInfo_NextAdjustment { get; set; }

        [DisplayName("Adj.Payment Change Date")]
        public string ArmInfo_AdjPaymentChangeDate { get; set; }

        [DisplayName("Adjustment frequency")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Enter valid amount.")]
        public double ArmInfo_AdjustmentFrequency { get; set; }

        [DisplayName("Floor")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Floor is not valid.")]
        public double ArmInfo_Floor { get; set; }

        [DisplayName("Index Name")]
        public string ArmInfo_IndexName { get; set; }

        [DisplayName("Margin")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Margin is not valid.")]
        public double ArmInfo_Margin { get; set; }

        [DisplayName("Ceiling")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Ceiling is not valid.")]
        public double ArmInfo_Ceiling { get; set; }

        public int ListingStatusID { get; set; }


        [Display(Name = "Is the note guaranteed?")]
        //[Required(ErrorMessage = "The Is Guaranteed field is required.")]
        public string IsGuaranteed { get; set; }

        public List<GuarantorInformation> GuarantorInformation { get; set; }

        [Display(Name = "Is Company")]
        public bool IsCompany { get; set; }

        [Display(Name = "Primary Borrower First Name")]
        [Required(ErrorMessage = "The Primary Borrower First Name field is required.")]
        public string PrimaryBorrowerFirstName { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "The Company Name field is required.")]
        public string CompanyName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; }

        [Display(Name = "Mailing Address")]
        [Required(ErrorMessage = "The Mailing Address field is required.")]
        public string MailingAddress { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "The Borrower City field is required.")]
        public string BorrowerCity { get; set; }

        [Display(Name = "State")]
        public string BorrowerState { get; set; }

        [Display(Name = "Zip")]
        public int BorrowerZip { get; set; }

        [Display(Name = "Home Ph")]
        public long BorrowerHomePh { get; set; }

        [Display(Name = "Work Ph")]
        public long BorrowerWorkPh { get; set; }

        [Display(Name = "Mobile Ph")]
        public long BorrowerMobilePh { get; set; }

        [Display(Name = "E-mail")]
        public string BorrowerEmail { get; set; }

        [Display(Name = "Fax")]
        public long BorrowerFax { get; set; }

        [Display(Name = "SS/Tax ID# - Primary borrower")]
        public long BorrowerSSTaxIDNoPrimary { get; set; }

    }

    [Serializable()]
    public class GuarantorInformation
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public long Id { get; set; }
        public long ListingDetailsId { get; set; }
    }


    [Serializable()]
    public class PropertyInformation
    {
        public long ID { get; set; }

        [Display(Name = "Property Type")]
        [Required(ErrorMessage = "The Property Type field is required.")]
        public int PropertyTypeID { get; set; }

        public List<CommonList> PropertyType { get; set; }                    // fetch from database.

        [Display(Name = "Property Address")]
        [Required(ErrorMessage = "The Property Address field is required.")]
        public string PropertyAddress { get; set; }

        [Display(Name = "Property City")]
        [Required(ErrorMessage = "The City field is required.")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Zip")]
        public int Zip { get; set; }

        [Display(Name = "Occupancy Status")]
        [Required(ErrorMessage = "The Occupancy Status field is required.")]
        public int OccupancyStatusID { get; set; }
        public List<CommonList> OccupancyStatus { get; set; }                    // fetch from database.

        [Display(Name = "State")]
        public string StateName { get; set; }

        [Display(Name = "Property Market Value")]
        [Required(ErrorMessage = "The Property Market Value field is required.")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Property Market Value is not valid.")]
        public double PropertyMarketValue { get; set; }

        [Display(Name = "Property Valuation Date")]
        public string PropertyValuationDate { get; set; }

        [Display(Name = "Valuation Type")]
        public int ValuationTypeID { get; set; }
        public List<CommonList> ValuationType { get; set; }                    // fetch from database.


        [Display(Name = "Is Company")]
        public bool IsCompany { get; set; }

        [Display(Name = "Primary Borrower First Name")]
       // [Required(ErrorMessage = "The Primary Borrower First Name field is required.")]
        public string PrimaryBorrowerFirstName { get; set; }

        [Display(Name = "Last Name")]
       // [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; }

        [Display(Name = "Mailing Address")]
       // [Required(ErrorMessage = "The Mailing Address field is required.")]
        public string MailingAddress { get; set; }

        [Display(Name = "City")]
       // [Required(ErrorMessage = "The Borrower City field is required.")]
        public string BorrowerCity { get; set; }

        [Display(Name = "State")]
        public string BorrowerState { get; set; }

        [Display(Name = "Zip")]
        public int BorrowerZip { get; set; }

        [Display(Name = "Home Ph")]
        public long BorrowerHomePh { get; set; }

        [Display(Name = "Work Ph")]
        public long BorrowerWorkPh { get; set; }

        [Display(Name = "Mobile Ph")]
        public long BorrowerMobilePh { get; set; }

        [Display(Name = "E-mail")]
        public string BorrowerEmail { get; set; }

        [Display(Name = "Fax")]
        public long BorrowerFax { get; set; }

        [Display(Name = "SS/Tax ID# - Primary borrower")]
        public long BorrowerSSTaxIDNoPrimary { get; set; }

        public int ListingStatusID { get; set; }
    }

    [Serializable()]
    public class CommentsInformation
    {
        public long ID { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Seller may consider offers starting at")]
        public bool IsSellerOffering { get; set; }

        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "Percentage is not valid.")]
        public double SellerOfferingPercentage { get; set; }

        public int ListingStatusID { get; set; }
    }

    [Serializable()]
    public class DocumentsInformation
    {
        public long ID { get; set; }

        [DisplayName("Post listing as active")]
        public int ListingStatusID { get; set; }

        [DisplayName("Feature this listing(costs will apply)*")]
        public bool IsSponsored { get; set; }
        public List<ListingLoanDocuments> ListingLoanDocuments { get; set; }

        public string ImagePath { get; set; }
        public List<ListingImage> ListingImages { get; set; }

        [Display(Name = "Copy of the NOTE and RIDER (if any)")]
        public string CopyOfNoteAndRider { get; set; }

        [Display(Name = "Copy of the recorded DEED OF TRUST or MORTGAGE")]
        public string CopyOfRecordedDeedTrust { get; set; }

        [Display(Name = "Copy of the LOAN MODIFICATION")]
        public string CopyLoanModification { get; set; }

        [Display(Name = "Copy of Appraisal or BPO")]
        public string CopyAppraisalBPO { get; set; }

        [Display(Name = "Payment History")]
        public string PaymentHistory { get; set; }

        [Display(Name = "Chain of Assignments.")]
        public int ChainAssignments { get; set; }
        public string ChainAssignmentsDoc { get; set; }
    }

    public enum YesNoGroup
    {
        No = 0, Yes = 1
    }

    public enum YesNoUnknownGroup
    {
        No = 0, Yes = 1, Unknown = 2
    }

    [Flags]
    public enum ChainOfAssignmentGroup
    {
        [Display(Name = "Yes")]
        Yes = 1,

        [Display(Name = "No")]
        No = 0,

        [Display(Name = "Never been assigned")]
        Never_been_Assigned = 2
    }

    [Flags]
    public enum CheckAllThatApplyGroup
    {
        [Display(Name = "1st Trust Deed/Mortgage")]
        FirstTrustDeedMortgage = 1,

        [Display(Name = "Junior Trust Deed/Mortgage")]
        JuniorTrustDeedMortgage = 2
    }

    [Serializable()]
    public class CommonList
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [Serializable()]
    public class ListingType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [Serializable()]
    public class ListingLoanDocuments
    {
        public long id { get; set; }
        public string FileName { get; set; }
        public bool IsDeleted { get; set; }
        public string DocumentType { get; set; }
        public int DocumentTypeID { get; set; }
    }

    [Serializable()]
    public class ListingImage
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFeatured { get; set; }
    }

}