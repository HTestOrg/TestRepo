using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataAccess
{
    public class InvestorListingsDA
    {
        #region Common variables
        private SqlCommand _cmd;
        #endregion

        #region Get Investor Listing Details
        public List<InvestorListingsModel> GetInvestorListingDetails(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            var token = new List<InvestorListingsModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand(); 
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetInvestorListings";
            _cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(investorListingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@LoanID", Convert.ToInt64(investorListingCriteria["LoanID"]));
            _cmd.Parameters.AddWithValue("@LienPositionID", Convert.ToInt64(investorListingCriteria["LienPositionID"]));
            _cmd.Parameters.AddWithValue("@LoanStatusID", Convert.ToInt64(investorListingCriteria["LoanStatusID"]));
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["State"])))
            {
                _cmd.Parameters.AddWithValue("@State", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@State", Convert.ToString(investorListingCriteria["State"]).Trim());
            } 
            _cmd.Parameters.AddWithValue("@LoanTypeID", Convert.ToInt64(investorListingCriteria["LoanTypeID"]));
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["Address"])))
            {
                _cmd.Parameters.AddWithValue("@Address", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Address", Convert.ToString(investorListingCriteria["Address"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@PropertyTypeID", Convert.ToInt64(investorListingCriteria["PropertyTypeID"])); 
            _cmd.Parameters.AddWithValue("@MinPricipalBalance", Convert.ToInt64(investorListingCriteria["MinPrincipalBalance"]));
            _cmd.Parameters.AddWithValue("@MaxPricipalBalance", Convert.ToInt64(investorListingCriteria["MaxPrincipalBalance"])); 
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["City"])))
            {
                _cmd.Parameters.AddWithValue("@City", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@City", Convert.ToString(investorListingCriteria["City"]).Trim());
            }
            if (double.IsNaN(Convert.ToDouble(investorListingCriteria["MinInterestRate"])))
            {
                _cmd.Parameters.AddWithValue("@MinInterestRate", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@MinInterestRate", Convert.ToDouble(investorListingCriteria["MinInterestRate"]));
            }
            if (double.IsNaN(Convert.ToDouble(investorListingCriteria["MaxInterestRate"])))
            {
                _cmd.Parameters.AddWithValue("@MaxInterestRate", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@MaxInterestRate", Convert.ToDouble(investorListingCriteria["MaxInterestRate"]));
            } 
            _cmd.Parameters.AddWithValue("@MinAskingPrice", Convert.ToDecimal(investorListingCriteria["MinAskingPrice"]));
            _cmd.Parameters.AddWithValue("@MaxAskingPrice", Convert.ToDecimal(investorListingCriteria["MaxAskingPrice"])); 
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["SortingOrder"])))
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", Convert.ToString(investorListingCriteria["SortingOrder"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(investorListingCriteria["Name"]).Trim());
            } 
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new InvestorListingsModel();
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]); 
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.BrokerId = Convert.ToInt64(dr["BrokerId"]);
                    temp.ListingType = Convert.ToString(dr["ListingType"]);
                    temp.Location = Convert.ToString(dr["Address"]);
                    temp.ActualLoanStatus = Convert.ToString(dr["ActualLoanStatus"]);
                    temp.IsFavorite = Convert.ToBoolean(dr["IsFavorite"]);
                    temp.BrokerName = Convert.ToString(dr["BrokerName"]);
                    temp.Status = Convert.ToString(dr["Status"]);
                    temp.CreatedDate = (dr["CreatedDate"] == DBNull.Value) ? Convert.ToDateTime(null) : Convert.ToDateTime(dr["CreatedDate"]);
                    temp.ModifiedDate = (dr["ModifiedDate"] == DBNull.Value) ? Convert.ToDateTime(null) : Convert.ToDateTime(dr["ModifiedDate"]);
                    temp.LienPositionName = Convert.ToString(dr["LienPositionName"]);
                    temp.NoteInterestRate = Convert.ToString(dr["NoteInterestRate"]);
                    temp.PrincipalBalance = Convert.ToString(dr["PrincipalBalance"]);
                    temp.SellerOfferingPercentage = Convert.ToString(dr["SellerOfferingPercentage"]);
                    temp.City = Convert.ToString(dr["City"]);
                    temp.StateName = Convert.ToString(dr["StateName"]); 
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Get User Image
        public string GetUserImage(Hashtable imageCriteria)
        {
            var _db = new DBUtility();
            var token = string.Empty;
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetUserImage";
            _cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(imageCriteria["UserID"]));
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    token = Convert.ToString(dr["ProfileImage"]);
                }
            }
            return token;
        }
        #endregion

        #region Get Broker Email Address
        public string GetBrokerEmailAddress(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            var token = string.Empty;
            var _dt = new DataTable();
            _cmd = new SqlCommand(); 
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetBrokerEmailAddress"; 
            if (Convert.ToInt64(investorListingCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@ListingId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ListingId", Convert.ToInt64(investorListingCriteria["ID"]));
            }
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    token = Convert.ToString(dr["EmailAddress"]);
                }
            }
            return token;
        }
        #endregion

        #region Get Listing Documents
        public List<ListingLoanDocuments> GetListingDocuments(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            var token = new List<ListingLoanDocuments>();
            var _dt = new DataTable();
            _cmd = new SqlCommand(); 
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetListingDocuments"; 
            if (Convert.ToInt64(investorListingCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@ListingId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ListingId", Convert.ToInt64(investorListingCriteria["ID"]));
            }
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var listingDocuments = new ListingLoanDocuments();
                    listingDocuments.id = Convert.ToInt64(dr["DocumentID"]);
                    listingDocuments.FileName = Convert.ToString(dr["DocumentName"]);
                    listingDocuments.DocumentTypeID = Convert.ToInt32(dr["DocumentTypeID"]); 
                    listingDocuments.DocumentType = Convert.ToString(dr["DocumentType"]);
                    token.Add(listingDocuments);
                }
            }
            return token;
        }
        #endregion

        #region Get Locations
        public List<Location> GetLocations()
        {
            var _db = new DBUtility();
            var token = new List<Location>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLocations"; 
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new Location();
                    temp.id = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Get Listing Types
        public List<ListingType> GetListingTypes()
        {
            var _db = new DBUtility();
            var token = new List<ListingType>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetListingTypes"; 
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new ListingType();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Get All Featured List
        public List<InvestorListingsModel> GetAllFeaturedList()
        {
            var _db = new DBUtility();
            var token = new List<InvestorListingsModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetAllFeaturedList"; 
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new InvestorListingsModel();
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.BrokerId = Convert.ToInt64(dr["BrokerId"]);
                    temp.ListingType = Convert.ToString(dr["ListingType"]);
                    temp.Location = Convert.ToString(dr["Address"]);
                    temp.ActualLoanStatus = Convert.ToString(dr["ActualLoanStatus"]);
                    temp.BrokerName = Convert.ToString(dr["BrokerName"]);
                    temp.Status = Convert.ToString(dr["Status"]);
                    temp.CreatedDate = (dr["CreatedDate"] == DBNull.Value) ? Convert.ToDateTime(null) : Convert.ToDateTime(dr["CreatedDate"]);
                    temp.ModifiedDate = (dr["ModifiedDate"] == DBNull.Value) ? Convert.ToDateTime(null) : Convert.ToDateTime(dr["ModifiedDate"]);
                    temp.LienPositionName = Convert.ToString(dr["LienPositionName"]);
                    temp.NoteInterestRate = Convert.ToString(dr["NoteInterestRate"]);
                    temp.PrincipalBalance = Convert.ToString(dr["PrincipalBalance"]);
                    temp.SellerOfferingPercentage = Convert.ToString(dr["SellerOfferingPercentage"]);
                    temp.City = Convert.ToString(dr["City"]);
                    temp.StateName = Convert.ToString(dr["StateName"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Get Single Listing Details
        public InvestorListingsModel GetSingleListingDetails(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            var token = new InvestorListingsModel();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetSingleInvestorListings";
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(investorListingCriteria["UserID"]).Trim());
            }
            if (Convert.ToInt64(investorListingCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@ListingId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ListingId", Convert.ToInt64(investorListingCriteria["ID"]));
            }
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new InvestorListingsModel();
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.AmountRequired = Convert.ToDouble(dr["AmountRequired"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.BrokerId = Convert.ToInt64(dr["BrokerId"]);
                    temp.ListingType = Convert.ToString(dr["ListingType"]);
                    temp.Location = Convert.ToString(dr["Location"]);
                    temp.IsFavorite = Convert.ToBoolean(dr["IsFavorite"]);
                    temp.BrokerName = Convert.ToString(dr["BrokerName"]);
                    temp.FirmName = Convert.ToString(dr["FirmName"]);
                    temp.Email = Convert.ToString(dr["Email"]);

                    temp.noteGeneralInfo = new NoteGeneralInformation();
                    temp.noteGeneralInfo.ID = Convert.ToInt64(dr["ID"]);
                    temp.noteGeneralInfo.LienPositionName = Convert.ToString(dr["LienPositionName"]);
                    temp.noteGeneralInfo.ActualLoanStatus = Convert.ToString(dr["ActualLoanStatus"]);
                    temp.noteGeneralInfo.LoanType = Convert.ToString(dr["LoanType"]);
                    temp.noteGeneralInfo.NumberOfPaymentsLast12Months = Convert.ToString(dr["NoOfPaymentsInLast12"]);
                    temp.noteGeneralInfo.PaidToDate = Convert.ToString(dr["PaidToDate"]);
                    temp.noteGeneralInfo.PropertyType = Convert.ToString(dr["PropertyType"]);
                    temp.noteGeneralInfo.LoanMaturityDate = Convert.ToString(dr["LoanMaturityDate"]);
                    temp.noteGeneralInfo.OriginalBalance = Convert.ToString(dr["OriginalBalance"]);
                    temp.noteGeneralInfo.PrincipalBalance = Convert.ToString(dr["PrincipalBalance"]);
                    temp.noteGeneralInfo.EstMarketValue = Convert.ToString(dr["EstMarketValue"]);
                    temp.noteGeneralInfo.PaymentAmount = Convert.ToString(dr["PaymentAmount"]);
                    temp.noteGeneralInfo.NoteInterestRate = Convert.ToString(dr["NoteInterestRate"]);
                    temp.noteGeneralInfo.SoldInterestRate = Convert.ToString(dr["SoldInterestRate"]);
              
                    temp.noteTermsTab = new NoteTermsTab();
                    temp.noteTermsTab.LienPositionName = Convert.ToString(dr["LienPositionName"]);
                    temp.noteTermsTab.AmortizationType = Convert.ToString(dr["AmortizationType"]);
                    temp.noteTermsTab.LoanType = Convert.ToString(dr["LoanType"]);
                    temp.noteTermsTab.OriginalBalance = Convert.ToString(dr["OriginalBalance"]); 
                    temp.noteTermsTab.CurrentBalance = Convert.ToString(dr["CurrentBalance"]); 
                    temp.noteTermsTab.PAndL = Convert.ToString(dr["PAndL"]);
                    temp.noteTermsTab.TotalPayment = Convert.ToString(dr["TotalPayment"]);
                    temp.noteTermsTab.NoteInterestRate = Convert.ToString(dr["NoteInterestRate"]);
                    temp.noteTermsTab.SoldInterestRate = Convert.ToString(dr["SoldInterestRate"]);
                    temp.noteTermsTab.TotalInTrust = Convert.ToString(dr["TotalInTrust"]);
                    temp.noteTermsTab.UnpaidInterest = Convert.ToString(dr["UnpaidInterest"]); 
                    temp.noteTermsTab.LateChargesIs = Convert.ToString(dr["LateChargesIs"]);
                    temp.noteTermsTab.UnpaidCharges = Convert.ToString(dr["UnpaidCharges"]);
                    temp.noteTermsTab.PrepaymentPenalty = Convert.ToString(dr["PrepaymentPenalty"]);
                    temp.noteTermsTab.RateType = Convert.ToString(dr["RateType"]);
                    temp.noteTermsTab.BalloonPayment = Convert.ToString(dr["BalloonPayment"]); 
                    temp.noteTermsTab.LoanTermsModified = Convert.ToString(dr["LoanTermsModified"]);
                    temp.noteTermsTab.Registered_wMERS = Convert.ToString(dr["Registered_wMERS"]);
                    temp.noteTermsTab.OnForbearancePlan = Convert.ToString(dr["OnForbearancePlan"]);
                    temp.noteTermsTab.InForeclosure = Convert.ToString(dr["InForeclosure"]);
                    temp.noteTermsTab.InBankruptcy = Convert.ToString(dr["InBankruptcy"]);
                    temp.noteTermsTab.SellerComments = Convert.ToString(dr["SellerComments"]);

                    temp.noteDatesTab = new NoteDatesTab();
                    temp.noteDatesTab.OriginationDate = Convert.ToString(dr["OriginationDate"]);
                    temp.noteDatesTab.FirstPaymentDate = Convert.ToString(dr["FirstPaymentDate"]);
                    temp.noteDatesTab.PaidToDate = Convert.ToString(dr["PaidToDate"]);
                    temp.noteDatesTab.NextDueDate = Convert.ToString(dr["NextDueDate"]);
                    temp.noteDatesTab.MaturityDate = Convert.ToString(dr["MaturityDate"]);
                    temp.noteDatesTab.LastPaymentReceived = Convert.ToString(dr["LastPaymentReceived"]);

                    temp.foreClosureTab = new ForeClosureTab();
                    temp.foreClosureTab.DateOpened = Convert.ToString(dr["DateOpened"]);
                    temp.foreClosureTab.ScheduleSaleDate = Convert.ToString(dr["ScheduleSaleDate"]);

                    temp.propertyTab = new PropertyTab();
                    temp.propertyTab.PropertyType = Convert.ToString(dr["PropertyType"]);
                    temp.propertyTab.City = Convert.ToString(dr["PropertyCity"]);
                    temp.propertyTab.MarketValue = Convert.ToString(dr["MarketValue"]);
                    temp.propertyTab.LTVRatio = Convert.ToString(dr["LTVRatio"]);
                    temp.propertyTab.Country = Convert.ToString(dr["Country"]);
                    temp.propertyTab.ValuationDate = Convert.ToString(dr["ValuationDate"]); 

                    token = temp;
                }
            }
            return token;
        }
        #endregion

        #region Mark As Favorite
        public int MarkAsFavorite(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand(); 
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_MarkAsFavorite";
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(investorListingCriteria["UserID"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@IsFavorite", Convert.ToBoolean(investorListingCriteria["IsFavorite"]));
            if (Convert.ToInt64(investorListingCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt64(investorListingCriteria["ID"]));
            } 
            return _db.ExecuteNonQuery(_cmd);
        }
        #endregion

        #region Send Message To Broker
        public long SendMessageToBroker(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_SendMessageToBroker";
            _cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(investorListingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingId", Convert.ToInt64(investorListingCriteria["ID"])); 
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["Message"])))
            {
                _cmd.Parameters.AddWithValue("@Message", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Message", Convert.ToString(investorListingCriteria["Message"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["Subject"])))
            {
                _cmd.Parameters.AddWithValue("@Subject", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Subject", Convert.ToString(investorListingCriteria["Subject"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["IsContactToBroker"])))
            {
                _cmd.Parameters.AddWithValue("@IsContactToBroker", false);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@IsContactToBroker", Convert.ToBoolean(investorListingCriteria["IsContactToBroker"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(investorListingCriteria["Name"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["PhoneNumber"])))
            {
                _cmd.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@PhoneNumber", Convert.ToString(investorListingCriteria["PhoneNumber"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["EmailAddress"])))
            {
                _cmd.Parameters.AddWithValue("@EmailAddress", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@EmailAddress", Convert.ToString(investorListingCriteria["EmailAddress"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@ReceiverId", Convert.ToInt64(investorListingCriteria["ReceiverId"]));
            _cmd.Parameters.Add("@CommentID", SqlDbType.BigInt);
            _cmd.Parameters["@CommentID"].Direction = ParameterDirection.Output; 
            var result = _db.ExecuteScalar(_cmd);
            return result;
        }
        #endregion
    }
}