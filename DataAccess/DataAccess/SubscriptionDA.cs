using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataAccess
{
    public class SubscriptionDA
    {
        #region Common Variables
        private SqlCommand _cmd;
        #endregion

        #region Get Payment Card types
        public List<PaymentCardTypes> GetPaymentCardTypes()
        {
            List<PaymentCardTypes> lstPaymentCardTypes = new List<PaymentCardTypes>();
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetPaymentCardTypes";

            var _dt = new DataTable();

            _dt = objUtility.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    PaymentCardTypes objCardType = new PaymentCardTypes();
                    objCardType.ID = Convert.ToInt32(dr["CardID"]);
                    objCardType.Name = Convert.ToString(dr["CardName"]);
                    lstPaymentCardTypes.Add(objCardType);
                }
            }
            return lstPaymentCardTypes;
        }
        #endregion

        #region Get Subscription Options for User
        public List<SubscriptionOption> GetSubscriptionOption(Hashtable criteria)
        {
            List<SubscriptionOption> listSubscriptionOption = new List<SubscriptionOption>();
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetSubscriptionOptions";

            _cmd.Parameters.AddWithValue("@RoleId", Convert.ToInt64(criteria["roleID"]));
            var _dt = new DataTable();

            _dt = objUtility.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    SubscriptionOption objSubscriptionOptionList = new SubscriptionOption();
                    objSubscriptionOptionList.ID = Convert.ToInt32(dr["ID"]);
                    objSubscriptionOptionList.PlanName = Convert.ToString(dr["PlanName"]);
                    objSubscriptionOptionList.Amount = Convert.ToDouble(dr["Amount"]);

                    listSubscriptionOption.Add(objSubscriptionOptionList);
                }
            }
            return listSubscriptionOption;
        }
        #endregion

        #region Save subscription Data
        public long SaveSubscriptionData(Hashtable subscriptionCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateSubscriptionData";

            if (string.IsNullOrWhiteSpace(Convert.ToString(subscriptionCriteria["Token"])))
            {
                _cmd.Parameters.AddWithValue("@Token", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Token", Convert.ToString(subscriptionCriteria["Token"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@Amount", Convert.ToDouble(subscriptionCriteria["Amount"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToString(subscriptionCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@BillingDate", Convert.ToString(subscriptionCriteria["BillingDate"]));
            _cmd.Parameters.AddWithValue("@ID", Convert.ToString(subscriptionCriteria["ID"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(subscriptionCriteria["CustomerID"])))
            {
                _cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@CustomerID", Convert.ToString(subscriptionCriteria["CustomerID"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@PlanID", Convert.ToString(subscriptionCriteria["PlanID"]));
            _cmd.Parameters.AddWithValue("@SubscriptionID", Convert.ToString(subscriptionCriteria["SubscriptionID"])); //stripe subscription ID
            _cmd.Parameters.AddWithValue("@ChargeID", Convert.ToString(subscriptionCriteria["ChargeID"]));

            //This is our inserted subscription tables ID
            _cmd.Parameters.Add("@Subscription_ID", SqlDbType.BigInt);
            _cmd.Parameters["@Subscription_ID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            int subscription_id = Convert.ToInt32(result);
            return result;
        }
        #endregion

        #region Get Plan Details
        public SubscriptionPlans GetPlanDetails(Hashtable subscriptionCriteria)
        {
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            var _db = new DBUtility();
            var temp = new SubscriptionPlans();
            var _dt = new DataTable();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetPlanDetails";

            _cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(subscriptionCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    temp.SubscriptionPlanID = Convert.ToString(dr["SubscriptionPlanID"]);
                    temp.Amount = Convert.ToDouble(dr["Amount"]);
                }
            }
            return temp;
        }
        #endregion

    }
}
