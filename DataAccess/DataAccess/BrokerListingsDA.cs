using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataAccess
{
    public class BrokerListingsDA
    {
        #region Common variables
        private SqlCommand _cmd;
        #endregion

        #region Get Broker Listing Details
        public List<BrokerListingsModel> GetBrokerListingDetails(Hashtable listingsCriteria)
        {
            var _db = new DBUtility();
            var token = new List<BrokerListingsModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetBrokerListings";

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
                    var temp = new BrokerListingsModel();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.AmountRequired = Convert.ToDouble(dr["AmountRequired"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.ListingType = Convert.ToString(dr["ListingType"]);
                    temp.ListingStatus = Convert.ToString(dr["ListingStatus"]);
                    temp.Location = Convert.ToString(dr["Location"]);
                    temp.MessageCount = Convert.ToInt32(dr["MessageCount"]);

                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Delete Current Listing
        public int DeleteCurrentListing(Hashtable listingsCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_DeleteCurrentListings";
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingsCriteria["UserID"]));
            }
            if (Convert.ToInt32(listingsCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(listingsCriteria["ID"]));
            }
            return _db.ExecuteNonQuery(_cmd);
        }
        #endregion

        #region Get Listing specific messages
        public DataTable GetListingSpecificMessages(string listingId, string userID) {
            var _dt = new DataTable();
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetCurrentListingMessages";
            if (string.IsNullOrWhiteSpace(userID))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", userID.Trim());
            }
            if (Convert.ToInt64(listingId) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt64(listingId));
            } 
            var result = _db.FillDataTable(_cmd, _dt);
            return result;
        }
        #endregion

        #region Get User replied Listing specific messages
        public DataTable GetUserRepliedListingSpecificMessages(long listingId, long userID, long CommentID)
        {
            var _dt = new DataTable();
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetUserRepliedListingMessages";
            if (userID <= 0)
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", userID);
            }
            if (listingId <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", listingId);
            }
            if (CommentID <= 0)
            {
                _cmd.Parameters.AddWithValue("@CommentID", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@CommentID", CommentID);
            }
            var result = _db.FillDataTable(_cmd, _dt);
            return result;
        }
        #endregion

        #region Get Current Listing Messages
        public List<ListingComments> GetCurrentListingMessages(Hashtable listingsCriteria)
        {
            var listingComments = new List<ListingComments>();
            var _dt = GetListingSpecificMessages(Convert.ToString(listingsCriteria["ID"]), Convert.ToString(listingsCriteria["UserID"])); 
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                { 
                    var comments = new ListingComments();
                    comments.CommentID = Convert.ToInt64(dr["CommentID"]);
                    comments.ListingId = Convert.ToInt64(dr["ListingId"]);
                    comments.ListingName = Convert.ToString(dr["ListingName"]);
                    comments.ImagePath = Convert.ToString(dr["ImagePath"]);
                    comments.MessageFrom = Convert.ToString(dr["MessageFrom"]); 
                    comments.MessageTimeStamp = Convert.ToString(dr["MessageTimeStamp"]);
                    comments.MessageText = Convert.ToString(dr["MessageText"]); 
                    comments.FromID = Convert.ToInt64(dr["FromID"]); 
                    var _repliedDt = GetUserRepliedListingSpecificMessages(comments.ListingId, comments.FromID, comments.CommentID);
                    if (_repliedDt.Rows.Count > 0)
                    {
                        var replComments = new List<ListingComments>();
                        foreach (DataRow replidDr in _repliedDt.Rows)
                        {   
                            var repliedComments = new ListingComments();
                            repliedComments.CommentID = Convert.ToInt64(dr["CommentID"]);
                            repliedComments.ListingId = Convert.ToInt64(replidDr["ListingId"]);
                            repliedComments.ListingName = Convert.ToString(replidDr["ListingName"]);
                            repliedComments.ImagePath = Convert.ToString(replidDr["ImagePath"]);
                            repliedComments.MessageFrom = Convert.ToString(replidDr["MessageFrom"]);
                            repliedComments.MessageTimeStamp = Convert.ToString(replidDr["MessageTimeStamp"]);
                            repliedComments.MessageText = Convert.ToString(replidDr["MessageText"]);
                            repliedComments.FromID = Convert.ToInt64(replidDr["FromID"]); 
                            replComments.Add(repliedComments);
                        }
                        comments.MessageCount = _repliedDt.Rows.Count;              // setting outer message count for displaying number of comments of logged in user.
                        comments.RepliedComments = replComments;
                    }
                    listingComments.Add(comments);
                } 
            }
            return listingComments;
        }
        #endregion

        #region Get list of Messages Details
        public List<MessageCenterModel> GetMessageListDetails(Hashtable listingsCriteria)
        {
            var _db = new DBUtility();
            var token = new List<MessageCenterModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetMessageCenterListing";

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
                    var temp = new MessageCenterModel();
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Subject = Convert.ToString(dr["CommentSubject"]);
                    temp.MessageBody = Convert.ToString(dr["MessageBody"]);
                    temp.IsRead = Convert.ToBoolean(dr["IsRead"]);
                    temp.ProfileImage = Convert.ToString(dr["ProfileImage"]);
                    temp.ToUser = Convert.ToString(dr["ToUser"]);
                    temp.FromUser = Convert.ToString(dr["FromUser"]);
                    temp.Duration = Convert.ToString(dr["Duration"]);
                    temp.Sender = Convert.ToInt64(dr["Sender"]);
                    temp.ListingDetailsID = Convert.ToInt64(dr["ListingDetailsID"]);
                    temp.IsArchived = Convert.ToBoolean(dr["IsArchived"]); 
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Reply To Message method
        public long ReplyToMessage(Hashtable listingsCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_ReplyToMessage";
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingsCriteria["UserID"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@ListingDetailsID", Convert.ToInt64(listingsCriteria["ID"]));
            _cmd.Parameters.AddWithValue("@ToUserId", Convert.ToInt64(listingsCriteria["ToUserID"]));
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["Message"])))
            {
                _cmd.Parameters.AddWithValue("@Message", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Message", Convert.ToString(listingsCriteria["Message"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["CommentSubject"])))
            {
                _cmd.Parameters.AddWithValue("@CommentSubject", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@CommentSubject", Convert.ToString(listingsCriteria["CommentSubject"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["ReplyToComment"])))
            {
                _cmd.Parameters.AddWithValue("@ReplyToComment", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ReplyToComment", Convert.ToString(listingsCriteria["ReplyToComment"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["IsContactToBroker"])))
            {
                _cmd.Parameters.AddWithValue("@IsContactToBroker", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@IsContactToBroker", Convert.ToBoolean(listingsCriteria["IsContactToBroker"]));
            } 
            _cmd.Parameters.Add("@CommentID", SqlDbType.BigInt);
            _cmd.Parameters["@CommentID"].Direction = ParameterDirection.Output;

            var result = _db.ExecuteScalar(_cmd);
            return result;
        }
        #endregion
         
        #region Delete Current Message
        public long ArchiveCurrentMessage(Hashtable listingsCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_ArchiveCurrentMessage";
            if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingsCriteria["UserID"]).Trim());
            }
            if (Convert.ToInt64(listingsCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt64(listingsCriteria["ID"]));
            }
            return _db.ExecuteNonQuery(_cmd);
        }
        #endregion

        //#region Delete Current Message
        //public long DeleteCurrentMessage(Hashtable listingsCriteria)
        //{
        //    var _db = new DBUtility();
        //    _cmd = new SqlCommand();
        //    _cmd.CommandType = CommandType.StoredProcedure;
        //    _cmd.CommandText = "GP_SP_DeleteCurrentMessage";
        //    if (string.IsNullOrWhiteSpace(Convert.ToString(listingsCriteria["UserID"])))
        //    {
        //        _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(listingsCriteria["UserID"]).Trim());
        //    }
        //    if (Convert.ToInt32(listingsCriteria["ID"]) <= 0)
        //    {
        //        _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
        //    }
        //    else
        //    {
        //        _cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(listingsCriteria["ID"]));
        //    }
        //    return _db.ExecuteNonQuery(_cmd);
        //}
        //#endregion

        #region Mark message as read
        public int MarkAsReadMessage(Hashtable listingsCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_MarkAsReadMessage";

            if (Convert.ToInt64(listingsCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt64(listingsCriteria["ID"]));
            }
            return _db.ExecuteNonQuery(_cmd);
        }
        #endregion

        #region Get Latest Listing Details for homepage
        public List<BrokerListingsModel> GetLatestBrokerListingDetailsForHomePage()
        {
            var _db = new DBUtility();
            var token = new List<BrokerListingsModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLatestListingsDetailsForHomePage";

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new BrokerListingsModel();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.AmountRequired = Convert.ToDouble(dr["AmountRequired"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.ListingType = Convert.ToString(dr["ListingType"]);
                    temp.ListingStatus = Convert.ToString(dr["ListingStatus"]);
                    temp.Location = Convert.ToString(dr["Location"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion
    }
}
