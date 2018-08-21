using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataAccess
{
    public class DashboardDA
    {
        #region Common variables
        private SqlCommand _cmd; 
        #endregion

        #region Get investor Dashboard
        public List<InvestorDashboardModel> GetInvestorDashboard(Hashtable listingsCriteria)
        { 
            var _db = new DBUtility();
            var token = new List<InvestorDashboardModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand(); 
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetInvestorDashboard"; 
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingsCriteria["UserID"]).Trim());
            } 
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new InvestorDashboardModel();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.AmountRequired = Convert.ToDouble(dr["AmountRequired"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.ListingType = Convert.ToString(dr["ListingType"]);
                    temp.IsFavorite = Convert.ToBoolean(dr["IsFavorite"]);
                    temp.Location = Convert.ToString(dr["Location"]);
                    temp.MessageCount = Convert.ToInt32(dr["MessageCount"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.SentMessageCount = Convert.ToInt32(dr["SentMessageCount"]); 
                    if (temp.IsFavorite)
                        temp.Status = "Favorite";
                    else
                        temp.Status = "Active";
                    token.Add(temp);
                }
            }
            return token; 
        }
        #endregion
    }
}
