using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static BusinessObjects.LearnModel;

namespace DataAccess.DataAccess
{
    public class LearnDA
    {
        #region Common Variables
        private SqlCommand _cmd;
        #endregion

        #region Get Experience Level
        public List<Experiences> GetExperienceLevel()
        {
            var _db = new DBUtility();
            var result = new List<Experiences>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetExperienceLevel";
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new Experiences();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    result.Add(temp);
                }
            }
            return result;
        }
        #endregion

        #region Get Topics
        public List<Topics> GetTopics(Hashtable learnCriteria)
        {
            var _db = new DBUtility();
            var result = new List<Topics>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetTopics";
            if (Convert.ToInt32(learnCriteria["Type"]) <= 0)
                _cmd.Parameters.AddWithValue("@CategoryID", 1);
            else
                _cmd.Parameters.AddWithValue("@CategoryID", Convert.ToInt32(learnCriteria["Type"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new Topics();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.TopicIcon = Convert.ToString(dr["TopicIcon"]);
                    result.Add(temp);
                }
            }
            return result;
        }
        #endregion

        #region Get Learn types
        public List<LearnType> GetLearnTypes()
        {
            var _db = new DBUtility();
            var result = new List<LearnType>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLearnTypes";
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new LearnType();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    result.Add(temp);
                }
            }
            return result;
        }
        #endregion

        #region Get Single Webinar Details
        public LearnModel GetSingleLearnDetails(Hashtable learnCriteria)
        {
            var _db = new DBUtility();
            var token = new LearnModel();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetSingleLearnDetails";
            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(learnCriteria["UserID"]).Trim());
            }
            if (Convert.ToInt32(learnCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(learnCriteria["ID"]));
            }
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new LearnModel();
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.ResourcePath = Convert.ToString(dr["ResourcePath"]);
                    temp.LearnTypeName = Convert.ToString(dr["LearnTypeName"]);
                    temp.TopicName = Convert.ToString(dr["TopicName"]);
                    temp.Url = Convert.ToString(dr["LearnUrl"]);
                    temp.ArticleDate = Convert.ToString(dr["ArticleDate"]);
                    temp.AuthorName = Convert.ToString(dr["AuthorName"]);
                    temp.LearnTypeID = Convert.ToInt32(dr["LearnTypeID"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.ExperienceLevelName = Convert.ToString(dr["ExperienceLevelName"]);
                    temp.TopicID = Convert.ToInt32(dr["TopicID"]);
                    token = temp;
                }
            }
            return token;
        }
        #endregion

        #region Save learn Data
        public long SaveLearnData(Hashtable learnCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateLearnData";


            _cmd.Parameters.AddWithValue("@LearnID", Convert.ToInt64(learnCriteria["LearnID"]));
            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(learnCriteria["Name"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["Description"])))
            {
                _cmd.Parameters.AddWithValue("@Description", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Description", Convert.ToString(learnCriteria["Description"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["Url"])))
            {
                _cmd.Parameters.AddWithValue("@Url", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Url", Convert.ToString(learnCriteria["Url"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@ContentType", Convert.ToInt32(learnCriteria["ContentType"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["AuthorName"])))
            {
                _cmd.Parameters.AddWithValue("@AuthorName", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@AuthorName", Convert.ToString(learnCriteria["AuthorName"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["ArticleDate"])))
            {
                _cmd.Parameters.AddWithValue("@ArticleDate", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ArticleDate", Convert.ToDateTime(learnCriteria["ArticleDate"]));
            }

            _cmd.Parameters.AddWithValue("@LearnTypeID", Convert.ToInt32(learnCriteria["LearnTypeID"]));
            _cmd.Parameters.AddWithValue("@ExperienceLevelID", Convert.ToInt32(learnCriteria["ExperienceLevelID"]));
            _cmd.Parameters.AddWithValue("@ListingStatusID", Convert.ToInt32(learnCriteria["ListingStatusID"]));
            _cmd.Parameters.AddWithValue("@TopicID", Convert.ToInt32(learnCriteria["TopicID"]));
            _cmd.Parameters.AddWithValue("@IsSponsored", Convert.ToBoolean(learnCriteria["IsSponsored"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToString(learnCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ContentFor", Convert.ToInt32(learnCriteria["ContentFor"]));

            _cmd.Parameters.Add("@ListID", SqlDbType.BigInt);
            _cmd.Parameters["@ListID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            int listid = Convert.ToInt32(result);

            return result;

        }
        #endregion

        #region Delete Current Listing
        public int DeleteCurrentResource(Hashtable learnCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_DeleteCurrentResource";
            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(learnCriteria["UserID"]));
            }
            if (Convert.ToInt32(learnCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(learnCriteria["ID"]));
            }

            return _db.ExecuteNonQuery(_cmd);
        }
        #endregion

        #region Get learn Listing Details
        public List<LearnModel> GetLearnListingDetails(Hashtable learnListingCriteria)
        {
            var _db = new DBUtility();
            var token = new List<LearnModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLearnListings";
            _cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(learnListingCriteria["UserID"]));

            if ((Convert.ToInt32(learnListingCriteria["ExperienceLevel"]) == 0))
            {
                _cmd.Parameters.AddWithValue("@ExperienceLevel", "");
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ExperienceLevel", Convert.ToString(learnListingCriteria["ExperienceLevel"]));
            }
            if ((Convert.ToInt32(learnListingCriteria["Topic"]) == 0))
            {
                _cmd.Parameters.AddWithValue("@Topic", "");
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Topic", Convert.ToString(learnListingCriteria["Topic"]));
            }


            if (string.IsNullOrWhiteSpace(Convert.ToString(learnListingCriteria["SearchText"])))
            {
                _cmd.Parameters.AddWithValue("@SearchText", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SearchText", Convert.ToString(learnListingCriteria["SearchText"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(learnListingCriteria["LearnType"])))
            {
                _cmd.Parameters.AddWithValue("@LearnType", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@LearnType", Convert.ToString(learnListingCriteria["LearnType"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(learnListingCriteria["SortingOrder"])))
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", Convert.ToString(learnListingCriteria["SortingOrder"]).Trim());
            }

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new LearnModel();
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Description = Convert.ToString(learnListingCriteria["IsAdminDashboard"]).Trim() == "1" ? "" : Convert.ToString(dr["Description"]).Length >= 50 ? Convert.ToString(dr["Description"]).Substring(0, 50) : Convert.ToString(dr["Description"]);
                    temp.ListingStatusID = Convert.ToInt32(dr["ListingStatusID"]);
                    temp.Status = Convert.ToString(dr["Status"]);
                    temp.Url = Convert.ToString(dr["LearnURL"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.TopicName = Convert.ToString(dr["TopicName"]);
                    temp.TopicID = Convert.ToInt32(dr["TopicID"]);
                    temp.LearnTypeName = Convert.ToString(dr["LearnTypeName"]);
                    temp.LearnTypeID = Convert.ToInt32(dr["LearnTypeID"]);
                    temp.ResourcePath = Convert.ToString(dr["ImagePath"]);
                    temp.CreatedDate = (dr["CreatedDate"] == DBNull.Value) ? Convert.ToDateTime(null) : Convert.ToDateTime(dr["CreatedDate"]);
                    temp.ModifiedDate = (dr["ModifiedDate"] == DBNull.Value) ? Convert.ToDateTime(null) : Convert.ToDateTime(dr["ModifiedDate"]);
                    temp.ContentTypeID = Convert.ToInt32(dr["ContentType"]);
                    temp.ContentForID = Convert.ToInt32(dr["ContentFor"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Save uploaded learn documents
        public long SaveUploadedLearnDocuments(Hashtable learnCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateLearnDocuments";

            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["FileName"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(learnCriteria["FileName"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@LearnID", Convert.ToInt64(learnCriteria["LearnID"]));
            _cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(learnCriteria["ID"]));
            _cmd.Parameters.AddWithValue("@IsDeleted", Convert.ToBoolean(learnCriteria["IsDeleted"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToBoolean(learnCriteria["userID"]));

            result = objUtility.ExecuteScalar(_cmd);
            return result;
        }
        #endregion

        #region Insert Update learn Image Path
        public string InsertUpdateLearnImagePath(Hashtable learnCriteria)
        {
            string result = "";
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_UpdateLearnImagePath";

            if (string.IsNullOrWhiteSpace(Convert.ToString(learnCriteria["FileName"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(learnCriteria["FileName"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@LearnID", Convert.ToInt64(learnCriteria["LearnID"]));
            result = objUtility.ExecuteNonQuery(_cmd).ToString();
            return result;
        }
        #endregion

        #region Get Latest News and Articles For Home Page
        public List<HomePageLatestResources> GetLatestNewsandArticlesForHomePage()
        {
            var _db = new DBUtility();
            var token = new List<HomePageLatestResources>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLatestNewsAndArticlesForHomePage";

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new HomePageLatestResources();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Title = Convert.ToString(dr["Name"]);
                    temp.Category = Convert.ToString(dr["Category"]);
                    temp.ImagePath = Convert.ToString(dr["ImageName"]);
                    temp.ContentType = Convert.ToInt32(dr["ContentType"]);
                    temp.ContentFor = Convert.ToInt32(dr["ContentFor"]);
                    temp.TopicID = Convert.ToInt32(dr["TopicID"]);
                    temp.LearnCategory = Convert.ToString(dr["LearnCategory"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Edit Current Resource
        public LearnModel EditCurrentResource(Hashtable learnCriteria)
        {
            var _db = new DBUtility();
            var temp = new LearnModel();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_EditCurrentResource";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(learnCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@LearnID", Convert.ToInt64(learnCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    temp.ID = Convert.ToInt64(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.ListingStatusID = Convert.ToInt32(dr["ListingStatusID"]);
                    temp.Url = Convert.ToString(dr["LearnURL"]);
                    temp.IsSponsored = Convert.ToBoolean(dr["IsSponsored"]);
                    temp.TopicName = Convert.ToString(dr["TopicName"]);
                    temp.TopicID = Convert.ToInt32(dr["TopicID"]);
                    temp.LearnTypeName = Convert.ToString(dr["LearnTypeName"]);
                    temp.AuthorName = Convert.ToString(dr["AuthorName"]);
                    temp.LearnTypeID = Convert.ToInt32(dr["LearnTypeID"]);
                    temp.ResourcePath = Convert.ToString(dr["ImagePath"]);
                    temp.ExperienceLevelID = Convert.ToString(dr["ExperienceLevel"]);
                    temp.ContentForID = Convert.ToInt32(dr["ContentFor"]);
                    temp.ContentTypeID = Convert.ToInt32(dr["ContentType"]);
                }
            }
            return temp;
        }
        #endregion

        #region To get Learn Documents
        public List<Documents> GetLearnDocuments(Hashtable learnCriteria)
        {
            var _db = new DBUtility();
            var temp = new List<Documents>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetLearnDocuments";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(learnCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@LearnID", Convert.ToInt64(learnCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var id = Convert.ToInt32(dr["ID"]);
                    var FileName = Convert.ToString(dr["Name"]);
                    temp.Add(new Documents { id = id, FileName = FileName });
                };
            }
            return temp;
        }
        #endregion
    }
}
