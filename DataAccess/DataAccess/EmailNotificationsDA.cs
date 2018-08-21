using BusinessObjects;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataAccess
{
    public class EmailNotificationsDA
    {
        #region Common variables
        private SqlCommand _cmd;
        #endregion

        #region Send Email Notification
        //public bool SendEmailNotification(Hashtable emailNotificationsCriteria)
        //{
        //    DBUtility _db = new DBUtility();
        //    _cmd = new SqlCommand(); 
        //    _cmd.CommandType = CommandType.StoredProcedure;
        //    _cmd.CommandText = "GP_SP_InsertIntoOutgoingEmailLog";  
        //    _cmd.Parameters.AddWithValue("@TemplateCode", Convert.ToString(emailNotificationsCriteria["TemplateCode"])); 
        //    if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailFrom"])))
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailFrom", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailFrom", Convert.ToString(emailNotificationsCriteria["EmailFrom"]).Trim());
        //    }

        //    if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailTo"])))
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailTo", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailTo", Convert.ToString(emailNotificationsCriteria["EmailTo"]).Trim());
        //    }

        //    if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailSubject"])))
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailSubject", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailSubject", Convert.ToString(emailNotificationsCriteria["EmailSubject"]).Trim());
        //    }

        //    if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailBody"])))
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailBody", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@EmailBody", Convert.ToString(emailNotificationsCriteria["EmailBody"]).Trim());
        //    }
        //    if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["Token"])))
        //    {
        //        _cmd.Parameters.AddWithValue("@Token", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@Token", Convert.ToString(emailNotificationsCriteria["Token"]).Trim());
        //    }

        //    var result = _db.ExecuteScalar(_cmd);
        //    if (result > 0)
        //        return true;
        //    return false; 
        //}
        #endregion

        #region Get html text for template to send email
        public string GetHtmlForTemplate(Hashtable emailNotificationsCriteria)
        {
            DBUtility _db = new DBUtility();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            var HtmlResult = ""; 
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetEmailTemplates"; 
            _cmd.Parameters.AddWithValue("@TemplateCode", Convert.ToString(emailNotificationsCriteria["TemplateCode"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                    HtmlResult = Convert.ToString(dr["HtmlBody"]);
            }
            return HtmlResult;
        }
        #endregion

        #region Get Notification Details
        public OutgoingEmailLogModel GetNotificationDetails(Hashtable emailNotificationsCriteria)
        {
            DBUtility _db = new DBUtility();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            var result = new OutgoingEmailLogModel();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetNotificationDetails";
            _cmd.Parameters.AddWithValue("@commentID", Convert.ToInt64(emailNotificationsCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    OutgoingEmailLogModel outgoingEmailLogModel = new OutgoingEmailLogModel();
                    outgoingEmailLogModel.MessageBody = Convert.ToString(dr["MessageBody"]);
                    outgoingEmailLogModel.CreatedDate = Convert.ToString(dr["CreatedDate"]);
                    outgoingEmailLogModel.EmailFrom = Convert.ToString(dr["EmailFrom"]);
                    outgoingEmailLogModel.EmailSubject = Convert.ToString(dr["EmailSubject"]);
                    outgoingEmailLogModel.EmailTo = Convert.ToString(dr["EmailTo"]);
                    outgoingEmailLogModel.ListingID = Convert.ToString(dr["ListingID"]);
                    outgoingEmailLogModel.DealName = Convert.ToString(dr["DealName"]);
                    outgoingEmailLogModel.MessageSender = Convert.ToString(dr["MessageSender"]);
                    outgoingEmailLogModel.ReceiverName = Convert.ToString(dr["ReceiverName"]);
                    outgoingEmailLogModel.PhoneNumber = Convert.ToString(dr["PhoneNumber"]);
                    result = outgoingEmailLogModel;
                }
            }
            return result;
        }
        #endregion

        #region Insert Into Outgoing Email Log
        public string InsertIntoOutgoingEmailLog(Hashtable emailNotificationsCriteria)
        {
            string result = "";
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertIntoOutgoingEmailLog";

            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["TemplateCode"])))
            {
                _cmd.Parameters.AddWithValue("@TemplateCode", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@TemplateCode", Convert.ToString(emailNotificationsCriteria["TemplateCode"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailFrom"])))
            {
                _cmd.Parameters.AddWithValue("@EmailFrom", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@EmailFrom", Convert.ToString(emailNotificationsCriteria["EmailFrom"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailSubject"])))
            {
                _cmd.Parameters.AddWithValue("@EmailSubject", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@EmailSubject", Convert.ToString(emailNotificationsCriteria["EmailSubject"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailTo"])))
            {
                _cmd.Parameters.AddWithValue("@EmailTo", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@EmailTo", Convert.ToString(emailNotificationsCriteria["EmailTo"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["EmailBody"])))
            {
                _cmd.Parameters.AddWithValue("@EmailBody", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@EmailBody", Convert.ToString(emailNotificationsCriteria["EmailBody"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["Token"])))
            {
                _cmd.Parameters.AddWithValue("@Token", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Token", Convert.ToString(emailNotificationsCriteria["Token"]).Trim());
            }
            result = objUtility.ExecuteNonQuery(_cmd).ToString();
            return result;
        }
        #endregion

        #region Get UserName From EmailAddress
        public string GetUserNameFromEmailAddress(Hashtable emailNotificationsCriteria)
        {
            DBUtility _db = new DBUtility();
            _cmd = new SqlCommand();
            var _dt = new DataTable();
            var result = "";

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetUserNameFromEmailAddress";
            if (string.IsNullOrWhiteSpace(Convert.ToString(emailNotificationsCriteria["Email"])))
            {
                _cmd.Parameters.AddWithValue("@Email", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Email", Convert.ToString(emailNotificationsCriteria["Email"]).Trim());
            }
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    result = Convert.ToString(dr["UserName"]);
                }
            }
            return result;
        }
        #endregion
    }
}