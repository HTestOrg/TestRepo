using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccess.DataAccess
{
    public class ListingDetailsDA
    {
        #region Common Variables
        private SqlCommand _cmd;
        #endregion

        #region Get listing loan types
        public List<ListingType> GetListingLoanTypes()
        {
            List<ListingType> lstListingtype = new List<ListingType>();
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetListingLoanTypes";

            var _dt = new DataTable();

            _dt = objUtility.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    ListingType objListing = new ListingType();
                    objListing.ID = Convert.ToInt32(dr["LoanTypeID"]);
                    objListing.Name = Convert.ToString(dr["LoanTypeName"]);
                    lstListingtype.Add(objListing);
                }
            }
            return lstListingtype;
        }
        #endregion

        #region Get listing loan types For Loan Information
        public LoanInformation GetListingDefaultValuesForLoanInformation()
        {
            LoanInformation lstListingModel = new LoanInformation();
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            var _ds = new DataSet();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetListingDefaultValuesForLoanInformation";
            _ds = objUtility.FillDataSet(_cmd, _ds);
            lstListingModel.RateType = new List<CommonList>();
            lstListingModel.AmortizationType = new List<CommonList>();
            lstListingModel.PaymentsFrequency = new List<CommonList>();
            lstListingModel.LienPositionType = new List<CommonList>();
            lstListingModel.JuniorTrustDeedORMortgageList = new List<CommonList>();

            for (int i = 0; i < _ds.Tables.Count; i++)
            {
                foreach (DataRow dr in _ds.Tables[i].Rows)
                {
                    CommonList temp = new CommonList();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    if (i == 0)
                        lstListingModel.RateType.Add(temp);
                    if (i == 1)
                        lstListingModel.AmortizationType.Add(temp);
                    if (i == 2)
                        lstListingModel.PaymentsFrequency.Add(temp);
                    if (i == 3)
                        lstListingModel.LienPositionType.Add(temp);
                    if (i == 4)
                        lstListingModel.JuniorTrustDeedORMortgageList.Add(temp);
                }
            }
            return lstListingModel;
        }
        #endregion

        #region Get listing loan types For Property Information
        public PropertyInformation GetListingDefaultValuesForPropertyInformation()
        {
            PropertyInformation lstListingModel = new PropertyInformation();
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            var _ds = new DataSet();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetListingDefaultValuesForPropertyInformation";
            _ds = objUtility.FillDataSet(_cmd, _ds);
            lstListingModel.PropertyType = new List<CommonList>();
            lstListingModel.OccupancyStatus = new List<CommonList>();
            lstListingModel.ValuationType = new List<CommonList>();
            for (int i = 0; i < _ds.Tables.Count; i++)
            {
                foreach (DataRow dr in _ds.Tables[i].Rows)
                {
                    CommonList temp = new CommonList();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    if (i == 0)
                        lstListingModel.PropertyType.Add(temp);
                    if (i == 1)
                        lstListingModel.OccupancyStatus.Add(temp);
                    if (i == 2)
                        lstListingModel.ValuationType.Add(temp);
                }
            }
            return lstListingModel;
        }
        #endregion

        #region Save Loan Information
        public long SaveLoanInformation(Hashtable listingCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateLoanInformation";
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(listingCriteria["Name"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@ListingTypeID", Convert.ToInt32(listingCriteria["ListingTypeID"]));
            _cmd.Parameters.AddWithValue("@ListingID", Convert.ToInt64(listingCriteria["ListingID"]));
            _cmd.Parameters.AddWithValue("@RateTypeID", Convert.ToInt32(listingCriteria["RateTypeID"]));

            _cmd.Parameters.AddWithValue("@AmortizationTypeID", Convert.ToInt32(listingCriteria["AmortizationTypeID"]));

            _cmd.Parameters.AddWithValue("@PrePayPenalty", Convert.ToBoolean(listingCriteria["PrePayPenalty"]));

            _cmd.Parameters.AddWithValue("@InDefault", Convert.ToBoolean(listingCriteria["InDefault"]));

            _cmd.Parameters.AddWithValue("@BalloonPayment", Convert.ToBoolean(listingCriteria["BalloonPayment"]));

            _cmd.Parameters.AddWithValue("@LoanTermsModified", Convert.ToBoolean(listingCriteria["LoanTermsModified"]));

            _cmd.Parameters.AddWithValue("@OnForbearance", Convert.ToBoolean(listingCriteria["OnForbearance"]));

            _cmd.Parameters.AddWithValue("@Registered_wMERS", Convert.ToBoolean(listingCriteria["Registered_wMERS"]));

            _cmd.Parameters.AddWithValue("@InForeclosure", Convert.ToString(listingCriteria["InForeclosure"]));
            _cmd.Parameters.AddWithValue("@InBankruptcy", Convert.ToString(listingCriteria["InBankruptcy"]));
            _cmd.Parameters.AddWithValue("@OriginationDate", Convert.ToString(listingCriteria["OriginationDate"]));
            _cmd.Parameters.AddWithValue("@PaidToDate", Convert.ToString(listingCriteria["PaidToDate"]));

            _cmd.Parameters.AddWithValue("@NextPaymentDate", Convert.ToString(listingCriteria["NextPaymentDate"]));
            _cmd.Parameters.AddWithValue("@PayersLastPaymentMadeDate", Convert.ToString(listingCriteria["PayersLastPaymentMadeDate"]));
            _cmd.Parameters.AddWithValue("@OriginalLoanAmount", Convert.ToDouble(listingCriteria["OriginalLoanAmount"]));
            _cmd.Parameters.AddWithValue("@UnpaidPrincipalBalance", Convert.ToDouble(listingCriteria["UnpaidPrincipalBalance"]));
            _cmd.Parameters.AddWithValue("@NoteMaturityDate", Convert.ToString(listingCriteria["NoteMaturityDate"]));

            _cmd.Parameters.AddWithValue("@AccruedLateCharges", Convert.ToDouble(listingCriteria["AccruedLateCharges"]));
            _cmd.Parameters.AddWithValue("@LoanCharges", Convert.ToDouble(listingCriteria["LoanCharges"]));
            _cmd.Parameters.AddWithValue("@NoOfPaymentsInLast12", Convert.ToInt32(listingCriteria["NoOfPaymentsInLast12"]));
            _cmd.Parameters.AddWithValue("@FirstPaymentDate", Convert.ToString(listingCriteria["FirstPaymentDate"]));
            _cmd.Parameters.AddWithValue("@EscrowImpounds", Convert.ToString(listingCriteria["EscrowImpounds"]));
            _cmd.Parameters.AddWithValue("@NoteInterestRate", Convert.ToDouble(listingCriteria["NoteInterestRate"]));

            _cmd.Parameters.AddWithValue("@SoldInterestRate", Convert.ToDouble(listingCriteria["SoldInterestRate"]));
            _cmd.Parameters.AddWithValue("@PaymentsFrequency", Convert.ToInt32(listingCriteria["PaymentsFrequency"]));
            _cmd.Parameters.AddWithValue("@LateCharges", Convert.ToDouble(listingCriteria["LateCharges"]));
            _cmd.Parameters.AddWithValue("@LateChargesType", Convert.ToInt32(listingCriteria["LateChargesType"]));
            _cmd.Parameters.AddWithValue("@LateChargesAfterXDays", Convert.ToInt32(listingCriteria["LateChargesAfterXDays"]));
            _cmd.Parameters.AddWithValue("@UnpaidInterest", Convert.ToDouble(listingCriteria["UnpaidInterest"]));
            _cmd.Parameters.AddWithValue("@PropertyTaxesDue", Convert.ToDouble(listingCriteria["PropertyTaxesDue"]));
            _cmd.Parameters.AddWithValue("@PaymentTrust", Convert.ToDouble(listingCriteria["PaymentTrust"]));

            _cmd.Parameters.AddWithValue("@pAndL", Convert.ToDouble(listingCriteria["pAndL"]));
            _cmd.Parameters.AddWithValue("@TotalMonthlyLoanPayment", Convert.ToDouble(listingCriteria["TotalMonthlyLoanPayment"]));
            _cmd.Parameters.AddWithValue("@CheckAllThatApply", Convert.ToInt32(listingCriteria["CheckAllThatApply"]));
            _cmd.Parameters.AddWithValue("@JuniorTrustDeedORMortgage", Convert.ToInt32(listingCriteria["JuniorTrustDeedORMortgage"]));
            _cmd.Parameters.AddWithValue("@LienPosition", Convert.ToInt32(listingCriteria["LienPosition"]));

            _cmd.Parameters.AddWithValue("@PrincipalBalance", Convert.ToDouble(listingCriteria["PrincipalBalance"]));
            _cmd.Parameters.AddWithValue("@MonthlyPayment", Convert.ToDouble(listingCriteria["MonthlyPayment"]));
            _cmd.Parameters.AddWithValue("@Modified", Convert.ToString(listingCriteria["Modified"]));
            _cmd.Parameters.AddWithValue("@CurrentStatus", Convert.ToString(listingCriteria["Current"]));
            _cmd.Parameters.AddWithValue("@DrawPeriodStartDate", Convert.ToString(listingCriteria["DrawPeriodStartDate"]));
            _cmd.Parameters.AddWithValue("@RepaymentPeriodStartDate", Convert.ToString(listingCriteria["RepaymentPeriodStartDate"]));
            _cmd.Parameters.AddWithValue("@InformationAsOf", Convert.ToString(listingCriteria["InformationAsOf"]));
            _cmd.Parameters.AddWithValue("@ArmInfo_NextAdjustment", Convert.ToString(listingCriteria["ArmInfo_NextAdjustment"]));
            _cmd.Parameters.AddWithValue("@ArmInfo_AdjPaymentChangeDate", Convert.ToString(listingCriteria["ArmInfo_AdjPaymentChangeDate"]));

            _cmd.Parameters.AddWithValue("@ArmInfo_AdjustmentFrequency", Convert.ToDouble(listingCriteria["ArmInfo_AdjustmentFrequency"]));
            _cmd.Parameters.AddWithValue("@ArmInfo_Floor", Convert.ToDouble(listingCriteria["ArmInfo_Floor"]));
            _cmd.Parameters.AddWithValue("@ArmInfo_IndexName", Convert.ToString(listingCriteria["ArmInfo_IndexName"]));
            _cmd.Parameters.AddWithValue("@ArmInfo_Margin", Convert.ToDouble(listingCriteria["ArmInfo_Margin"]));
            _cmd.Parameters.AddWithValue("@ArmInfo_Ceiling", Convert.ToDouble(listingCriteria["ArmInfo_Ceiling"]));
            _cmd.Parameters.AddWithValue("@ListingStatusID", Convert.ToInt32(listingCriteria["ListingStatusID"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToString(listingCriteria["UserID"]));

            DataTable GuarantorInformationTbl = CreateTable();
            if(listingCriteria["GuarantorInformation"]!=null)
            {
                foreach (var gInfo in (List<GuarantorInformation>)listingCriteria["GuarantorInformation"])
                {
                    GuarantorInformationTbl.Rows.Add(gInfo.Name.Trim(), gInfo.Address.Trim(), gInfo.Phone.Trim());
                }
               
            }

            _cmd.Parameters.AddWithValue("@Guarantor", GuarantorInformationTbl);
            _cmd.Parameters.Add("@ListID", SqlDbType.BigInt);

            //Save Brower information
            _cmd.Parameters.AddWithValue("@IsCompany", Convert.ToBoolean(listingCriteria["IsCompany"]));
            _cmd.Parameters.AddWithValue("@PrimaryBorrowerFirstName", Convert.ToBoolean(listingCriteria["IsCompany"])? Convert.ToString(listingCriteria["CompanyName"]) : Convert.ToString(listingCriteria["PrimaryBorrowerFirstName"]));
            _cmd.Parameters.AddWithValue("@LastName", Convert.ToString(listingCriteria["LastName"]));
            _cmd.Parameters.AddWithValue("@MailingAddress", Convert.ToString(listingCriteria["MailingAddress"]));
            _cmd.Parameters.AddWithValue("@BorrowerCity", Convert.ToString(listingCriteria["BorrowerCity"]));
            _cmd.Parameters.AddWithValue("@BorrowerState", Convert.ToString(listingCriteria["BorrowerState"]));
            _cmd.Parameters.AddWithValue("@BorrowerZip", Convert.ToInt32(listingCriteria["BorrowerZip"]));
            _cmd.Parameters.AddWithValue("@BorrowerHomePh", Convert.ToInt64(listingCriteria["BorrowerHomePh"]));
            _cmd.Parameters.AddWithValue("@BorrowerWorkPh", Convert.ToInt64(listingCriteria["BorrowerWorkPh"]));
            _cmd.Parameters.AddWithValue("@BorrowerMobilePh", Convert.ToInt64(listingCriteria["BorrowerMobilePh"]));
            _cmd.Parameters.AddWithValue("@BorrowerEmail", Convert.ToString(listingCriteria["BorrowerEmail"]));
            _cmd.Parameters.AddWithValue("@BorrowerFax", Convert.ToInt64(listingCriteria["BorrowerFax"]));
            _cmd.Parameters.AddWithValue("@BorrowerSSTaxIDNoPrimary", Convert.ToInt64(listingCriteria["BorrowerSSTaxIDNoPrimary"]));


            _cmd.Parameters["@ListID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            return Convert.ToInt64(result);
        }
        #endregion

        #region Save Property Information
        public long SavePropertyInformation(Hashtable listingCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdatePropertyInformation";
            _cmd.Parameters.AddWithValue("@ListingID", Convert.ToInt64(listingCriteria["ListingID"]));
            _cmd.Parameters.AddWithValue("@PropertyTypeID", Convert.ToInt32(listingCriteria["PropertyTypeID"])); 
            _cmd.Parameters.AddWithValue("@PropertyAddress", Convert.ToString(listingCriteria["PropertyAddress"]));
            _cmd.Parameters.AddWithValue("@City", Convert.ToString(listingCriteria["City"]));
            _cmd.Parameters.AddWithValue("@Country", Convert.ToString(listingCriteria["Country"]));
            _cmd.Parameters.AddWithValue("@Zip", Convert.ToInt32(listingCriteria["Zip"]));
            _cmd.Parameters.AddWithValue("@OccupancyStatusID", Convert.ToInt32(listingCriteria["OccupancyStatusID"]));
            _cmd.Parameters.AddWithValue("@StateName", Convert.ToString(listingCriteria["StateName"]));
            _cmd.Parameters.AddWithValue("@PropertyMarketValue", Convert.ToDouble(listingCriteria["PropertyMarketValue"]));
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingCriteria["PropertyValuationDate"])))
            {
                _cmd.Parameters.AddWithValue("@PropertyValuationDate", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@PropertyValuationDate", Convert.ToString(listingCriteria["PropertyValuationDate"]));
            }
            _cmd.Parameters.AddWithValue("@ValuationTypeID", Convert.ToInt32(listingCriteria["ValuationTypeID"]));
            //_cmd.Parameters.AddWithValue("@IsCompany", Convert.ToBoolean(listingCriteria["IsCompany"]));
            //_cmd.Parameters.AddWithValue("@PrimaryBorrowerFirstName", Convert.ToString(listingCriteria["PrimaryBorrowerFirstName"]));
            //_cmd.Parameters.AddWithValue("@LastName", Convert.ToString(listingCriteria["LastName"]));
            //_cmd.Parameters.AddWithValue("@MailingAddress", Convert.ToString(listingCriteria["MailingAddress"]));
            //_cmd.Parameters.AddWithValue("@BorrowerCity", Convert.ToString(listingCriteria["BorrowerCity"]));
            //_cmd.Parameters.AddWithValue("@BorrowerState", Convert.ToString(listingCriteria["BorrowerState"]));
            //_cmd.Parameters.AddWithValue("@BorrowerZip", Convert.ToInt32(listingCriteria["BorrowerZip"]));
            //_cmd.Parameters.AddWithValue("@BorrowerHomePh", Convert.ToInt64(listingCriteria["BorrowerHomePh"]));
            //_cmd.Parameters.AddWithValue("@BorrowerWorkPh", Convert.ToInt64(listingCriteria["BorrowerWorkPh"]));
            //_cmd.Parameters.AddWithValue("@BorrowerMobilePh", Convert.ToInt64(listingCriteria["BorrowerMobilePh"]));
            //_cmd.Parameters.AddWithValue("@BorrowerEmail", Convert.ToString(listingCriteria["BorrowerEmail"]));
            //_cmd.Parameters.AddWithValue("@BorrowerFax", Convert.ToInt64(listingCriteria["BorrowerFax"]));
            //_cmd.Parameters.AddWithValue("@BorrowerSSTaxIDNoPrimary", Convert.ToInt64(listingCriteria["BorrowerSSTaxIDNoPrimary"]));
            _cmd.Parameters.AddWithValue("@ListingStatusID", Convert.ToInt32(listingCriteria["ListingStatusID"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.Add("@ListID", SqlDbType.BigInt);
            _cmd.Parameters["@ListID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            return Convert.ToInt64(result);
        }
        #endregion

        #region Save Comments Information
        public long SaveCommentsInformation(Hashtable listingCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateCommentsInformation";
            _cmd.Parameters.AddWithValue("@ListingID", Convert.ToInt64(listingCriteria["ListingID"]));
            _cmd.Parameters.AddWithValue("@Comments", Convert.ToString(listingCriteria["Comments"]));
            _cmd.Parameters.AddWithValue("@IsSellerOffering", Convert.ToBoolean(listingCriteria["IsSellerOffering"]));
            _cmd.Parameters.AddWithValue("@SellerOfferingPercentage", Convert.ToDouble(listingCriteria["SellerOfferingPercentage"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingStatusID", Convert.ToInt32(listingCriteria["ListingStatusID"]));
            _cmd.Parameters.Add("@ListID", SqlDbType.BigInt);
            _cmd.Parameters["@ListID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            return Convert.ToInt64(result);
        }
        #endregion

        #region Save Documents Information
        public long SaveDocumentsInformation(Hashtable listingCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateDocumentsInformation";
            _cmd.Parameters.AddWithValue("@ListingID", Convert.ToInt64(listingCriteria["ListingID"]));
            _cmd.Parameters.AddWithValue("@ListingStatusID", Convert.ToInt32(listingCriteria["ListingStatusID"]));
            _cmd.Parameters.AddWithValue("@IsSponsored", Convert.ToBoolean(listingCriteria["IsSponsored"]));
            _cmd.Parameters.AddWithValue("@ChainAssignments", Convert.ToInt32(listingCriteria["ChainAssignments"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.Add("@ListID", SqlDbType.BigInt);
            _cmd.Parameters["@ListID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            return Convert.ToInt64(result);
        }
        #endregion

        #region Save uploaded listing documents
        public long SaveUploadedListingDocuments(Hashtable investorListingCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateListingDocuments";

            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["FileName"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(investorListingCriteria["FileName"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@ListingID", Convert.ToInt64(investorListingCriteria["ListingID"]));
            _cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(investorListingCriteria["ID"]));
            _cmd.Parameters.AddWithValue("@DocumentTypeID", Convert.ToInt32(investorListingCriteria["DocumentTypeID"]));
            _cmd.Parameters.AddWithValue("@IsDeleted", Convert.ToBoolean(investorListingCriteria["IsDeleted"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(investorListingCriteria["UserID"]));

            result = objUtility.ExecuteScalar(_cmd);
            return result;
        }
        #endregion

        #region Insert Update Image Path
        public string InsertUpdateListingImagePath(Hashtable investorListingCriteria)
        {
            string result = "";
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateListingImagePath";

            if (string.IsNullOrWhiteSpace(Convert.ToString(investorListingCriteria["FileName"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(investorListingCriteria["FileName"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@ListingID", Convert.ToInt64(investorListingCriteria["ListingID"]));
            _cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(investorListingCriteria["ID"]));
            _cmd.Parameters.AddWithValue("@IsDeleted", Convert.ToBoolean(investorListingCriteria["IsDeleted"]));
            _cmd.Parameters.AddWithValue("@IsFeatured", Convert.ToBoolean(investorListingCriteria["IsFeatured"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(investorListingCriteria["UserID"]));
            result = objUtility.ExecuteScalar(_cmd).ToString();
            return result;
        }
        #endregion

        #region To Get Loan Information Details
        public LoanInformation GetLoanInformationDetails(Hashtable listingCriteria)
        {
            var _db = new DBUtility();
            var temp = new LoanInformation();
            var _dt = new DataTable();
            var _ds = new DataSet();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLoanInformationDetails";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(listingCriteria["ID"]));

            _ds = _db.FillDataSet(_cmd, _ds);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _ds.Tables[0].Rows)
                {
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.ListingTypeID = Convert.ToInt32(dr["ListingTypeID"]);
                    temp.RateTypeID = Convert.ToInt32(dr["RateTypeID"]);
                    temp.AmortizationTypeID = Convert.ToInt32(dr["AmortizationTypeID"]);
                    temp.PrePayPenalty = Convert.ToBoolean(dr["PrePayPenalty"]);
                    temp.InDefault = Convert.ToBoolean(dr["InDefault"]);
                    temp.BalloonPayment = Convert.ToBoolean(dr["BalloonPayment"]);
                    temp.LoanTermsModified = Convert.ToBoolean(dr["LoanTermsModified"]);
                    temp.OnForbearance = Convert.ToBoolean(dr["OnForbearance"]);
                    temp.Registered_wMERS = Convert.ToBoolean(dr["Registered_wMERS"]);
                    temp.InForeclosureID = Convert.ToString(dr["InForeclosure"]);
                    temp.InBankruptcyID = Convert.ToString(dr["InBankruptcy"]);
                    temp.OriginationDate = Convert.ToString(dr["OriginationDate"]);
                    temp.PaidToDate = Convert.ToString(dr["PaidToDate"]);
                    temp.NextPaymentDate = Convert.ToString(dr["NextPaymentDate"]);
                    temp.PayersLastPaymentMadeDate = Convert.ToString(dr["PayersLastPaymentMadeDate"]);
                    temp.OriginalLoanAmount = Convert.ToDouble(dr["OriginalLoanAmount"]);
                    temp.UnpaidPrincipalBalance = Convert.ToDouble(dr["UnpaidPrincipalBalance"]);
                    temp.NoteMaturityDate = Convert.ToString(dr["NoteMaturityDate"]);
                    temp.AccruedLateCharges = Convert.ToDouble(dr["AccruedLateCharges"]);
                    temp.LoanCharges = Convert.ToDouble(dr["LoanCharges"]);
                    temp.NoOfPaymentsInLast12 = Convert.ToInt32(dr["NoOfPaymentsInLast12"]);
                    temp.FirstPaymentDate = Convert.ToString(dr["FirstPaymentDate"]);
                    temp.EscrowImpounds = Convert.ToString(dr["EscrowImpounds"]);
                    temp.NoteInterestRate = Convert.ToDouble(dr["NoteInterestRate"]);
                    temp.SoldInterestRate = Convert.ToDouble(dr["SoldInterestRate"]);
                    temp.PaymentsFrequencyID = Convert.ToInt32(dr["PaymentsFrequency"]);
                    temp.LateCharges = Convert.ToDouble(dr["LateCharges"]);
                    temp.LateChargesType = Convert.ToInt32(dr["LateChargesType"]);
                    temp.LateChargesAfterXDays = Convert.ToInt32(dr["LateChargesAfterXDays"]);
                    temp.UnpaidInterest = Convert.ToDouble(dr["UnpaidInterest"]);
                    temp.PropertyTaxesDue = Convert.ToDouble(dr["PropertyTaxesDue"]);
                    temp.PaymentTrust = Convert.ToDouble(dr["PaymentTrust"]);

                    temp.pAndL = Convert.ToDouble(dr["pAndL"]);
                    temp.TotalMonthlyLoanPayment = Convert.ToInt32(dr["TotalMonthlyLoanPayment"]);

                    temp.CheckAllThatApplyID = Convert.ToInt32(dr["CheckAllThatApply"]);
                    temp.JuniorTrustDeedORMortgageID = Convert.ToInt32(dr["JuniorTrustDeedORMortgage"]);
                    temp.LienPositionID = Convert.ToInt32(dr["LienPosition"]);
                    temp.PrincipalBalance = Convert.ToDouble(dr["PrincipalBalance"]);
                    temp.MonthlyPayment = Convert.ToDouble(dr["MonthlyPayment"]);
                    temp.InformationAsOf = Convert.ToString(dr["InformationAsOf"]);
                    temp.Modified = Convert.ToString(dr["Modified"]);
                    temp.Current = Convert.ToString(dr["CurrentStatus"]);
                    temp.DrawPeriodStartDate = Convert.ToString(dr["DrawPeriodStartDate"]);
                    temp.RepaymentPeriodStartDate = Convert.ToString(dr["RepaymentPeriodStartDate"]);

                    temp.ArmInfo_NextAdjustment = Convert.ToString(dr["ArmInfo_NextAdjustment"]);
                    temp.ArmInfo_AdjPaymentChangeDate = Convert.ToString(dr["ArmInfo_AdjPaymentChangeDate"]);
                    temp.ArmInfo_AdjustmentFrequency = Convert.ToDouble(dr["ArmInfo_AdjustmentFrequency"]);
                    temp.ArmInfo_Floor = Convert.ToDouble(dr["ArmInfo_Floor"]);
                    temp.ArmInfo_IndexName = Convert.ToString(dr["ArmInfo_IndexName"]);
                    temp.ArmInfo_Margin = Convert.ToDouble(dr["ArmInfo_Margin"]);
                    temp.ArmInfo_Ceiling = Convert.ToDouble(dr["ArmInfo_Ceiling"]);

                    temp.IsCompany= Convert.ToBoolean(dr["IsCompany"]);
                    temp.PrimaryBorrowerFirstName = dr["PrimaryBorrowerFirstName"].ToString();
                    temp.LastName= dr["LastName"].ToString();
                    temp.MailingAddress= dr["MailingAddress"].ToString();
                    temp.BorrowerCity= dr["BorrowerCity"].ToString();
                    temp.BorrowerState= dr["BorrowerState"].ToString();
                    temp.BorrowerZip= Convert.ToInt32(dr["BorrowerZip"]);
                    temp.BorrowerHomePh= Convert.ToInt64(dr["BorrowerHomePh"]);
                    temp.BorrowerWorkPh= Convert.ToInt64(dr["BorrowerWorkPh"]);
                    temp.BorrowerMobilePh= Convert.ToInt64(dr["BorrowerMobilePh"]);
                    temp.BorrowerEmail= dr["BorrowerEmail"].ToString();
                    temp.BorrowerFax= Convert.ToInt64(dr["BorrowerFax"]);
                    temp.BorrowerSSTaxIDNoPrimary = Convert.ToInt64(dr["BorrowerSSTaxIDNoPrimary"]);
                    temp.CompanyName = dr["PrimaryBorrowerFirstName"].ToString();


                    temp.ListingStatusID = Convert.ToInt32(dr["ListingStatusID"]);
                }
            }

            if (_ds.Tables.Count>1)
            {
                temp.GuarantorInformation = (from DataRow dr in _ds.Tables[1].AsEnumerable()
                                             select new GuarantorInformation()
                                             {
                                                 Name = dr["Name"].ToString(),
                                                 Address = dr["Address"].ToString(),
                                                 Phone = dr["Phone"].ToString()
                                             }).ToList();
                temp.IsGuaranteed = temp.GuarantorInformation.Count > 0 ? "Yes" : "No";

                //(List<GuarantorInformation>)_ds.Tables[1].AsEnumerable().ToList();
            }
            return temp;
        }
        #endregion

        #region To Get Property Information Details
        public PropertyInformation GetPropertyInformationDetails(Hashtable listingCriteria)
        {
            var _db = new DBUtility();
            var temp = new PropertyInformation();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetPropertyInformationDetails";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(listingCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.PropertyTypeID = (Convert.ToString(dr["PropertyTypeID"])) == "" ? 0 : Convert.ToInt32(dr["PropertyTypeID"]);
                    temp.PropertyAddress = Convert.ToString(dr["PropertyAddress"]);

                    temp.City = Convert.ToString(dr["City"]);
                    temp.Country = Convert.ToString(dr["Country"]);
                    temp.Zip = Convert.ToInt32(dr["Zip"]);
                    temp.OccupancyStatusID = (Convert.ToString(dr["OccupancyStatusID"])) == "" ? 0 : Convert.ToInt32(dr["OccupancyStatusID"]);
                    temp.StateName = Convert.ToString(dr["StateName"]);
                    temp.PropertyMarketValue = Convert.ToDouble(dr["PropertyMarketValue"]);
                    temp.PropertyValuationDate = Convert.ToString(dr["PropertyValuationDate"]);
                    temp.ValuationTypeID = (Convert.ToString(dr["ValuationTypeID"])) == "" ? 0 : Convert.ToInt32(dr["ValuationTypeID"]);
                    temp.IsCompany = Convert.ToBoolean(dr["IsCompany"]);
                    temp.PrimaryBorrowerFirstName = Convert.ToString(dr["PrimaryBorrowerFirstName"]);
                    temp.LastName = Convert.ToString(dr["LastName"]);
                    temp.MailingAddress = Convert.ToString(dr["MailingAddress"]);
                    temp.BorrowerCity = Convert.ToString(dr["BorrowerCity"]);

                    temp.BorrowerState = Convert.ToString(dr["BorrowerState"]);
                    temp.BorrowerZip = Convert.ToInt32(dr["BorrowerZip"]);
                    temp.BorrowerHomePh = Convert.ToInt64(dr["BorrowerHomePh"]);
                    temp.BorrowerWorkPh = Convert.ToInt64(dr["BorrowerWorkPh"]);
                    temp.BorrowerMobilePh = Convert.ToInt64(dr["BorrowerMobilePh"]);
                    temp.BorrowerEmail = Convert.ToString(dr["BorrowerEmail"]);
                    temp.BorrowerFax = Convert.ToInt64(dr["BorrowerFax"]);
                    temp.BorrowerSSTaxIDNoPrimary = Convert.ToInt64(dr["BorrowerSSTaxIDNoPrimary"]);
                    temp.ListingStatusID = Convert.ToInt32(dr["ListingStatusID"]);
                }
            }
            return temp;
        }
        #endregion

        #region To Get Comment Information Details
        public CommentsInformation GetCommentInformationDetails(Hashtable listingCriteria)
        {
            var _db = new DBUtility();
            var temp = new CommentsInformation();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetCommentInformationDetails";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(listingCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Comments = Convert.ToString(dr["Comments"]);
                    temp.IsSellerOffering = Convert.ToBoolean(dr["IsSellerOffering"]);
                    temp.SellerOfferingPercentage = Convert.ToDouble(dr["SellerOfferingPercentage"]);
                    temp.ListingStatusID = Convert.ToInt32(dr["ListingStatusID"]);
                }
            }
            return temp;
        }
        #endregion

        #region To Get Document Information Details
        public DocumentsInformation GetDocumentInformationDetails(Hashtable listingCriteria)
        {
            var _db = new DBUtility();
            var temp = new DocumentsInformation();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetDocumentInformationDetails";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(listingCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.ListingStatusID = Convert.ToInt32(dr["ListingStatusID"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.ChainAssignments = Convert.ToInt32(dr["ChainAssignments"]);
                }
            }
            return temp;
        }
        #endregion  

        #region To Get Loan Images
        public List<ListingImage> GetLoanImages(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            var temp = new List<ListingImage>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLoanImages";
            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(investorListingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(investorListingCriteria["ID"]));


            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var id = Convert.ToInt32(dr["ID"]);
                    var FileName = Convert.ToString(dr["Name"]);
                    var isFeatured = Convert.ToBoolean(dr["IsFeatured"]);
                    temp.Add(new ListingImage { id = id, FileName = FileName, IsFeatured = isFeatured });
                };
            }
            return temp;
        }
        #endregion

        #region To get specific loan listing documents details
        public List<ListingLoanDocuments> GetLoanSpecificListingLoanDocuments(Hashtable investorListingCriteria)
        {
            var _db = new DBUtility();
            var temp = new List<ListingLoanDocuments>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetBrokerSpecificListingLoanDocuments";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(investorListingCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(investorListingCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var id = Convert.ToInt64(dr["ID"]);
                    var FileName = Convert.ToString(dr["Name"]);
                    var DocumentType = Convert.ToString(dr["DocumentType"]);
                    var DocumentTypeID = Convert.ToInt32(dr["DocumentTypeID"]);
                    temp.Add(new ListingLoanDocuments { id = id, FileName = FileName, DocumentType = DocumentType, DocumentTypeID = DocumentTypeID });
                };
            }
            return temp;
        }
        #endregion

       private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            return dt;
        }

    }
}