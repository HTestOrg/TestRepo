using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataAccess
{
    public class TestimonialDA
    {
        #region Common Variables
        private SqlCommand _cmd;
        #endregion

        #region Get all Testimonial
        public List<TestimonialModel> GetallTestimonial(Hashtable testimonialCriteria)
        {
            var _db = new DBUtility();
            var token = new List<TestimonialModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetAllTestimonial";

            if (string.IsNullOrWhiteSpace(Convert.ToString(testimonialCriteria["SearchText"])))
            {
                _cmd.Parameters.AddWithValue("@SearchText", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SearchText", Convert.ToString(testimonialCriteria["SearchText"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(testimonialCriteria["SortingOrder"])))
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", Convert.ToString(testimonialCriteria["SortingOrder"]).Trim());
            }

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new TestimonialModel();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Author = Convert.ToString(dr["Author"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    temp.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    temp.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Save Testimonial Data
        public long SaveTestimonialData(Hashtable testimonialCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUpdateTestimonialData";

            _cmd.Parameters.AddWithValue("ID", Convert.ToInt32(testimonialCriteria["ID"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(testimonialCriteria["Author"])))
            {
                _cmd.Parameters.AddWithValue("@Author", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Author", Convert.ToString(testimonialCriteria["Author"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(testimonialCriteria["Description"])))
            {
                _cmd.Parameters.AddWithValue("@Description", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Description", Convert.ToString(testimonialCriteria["Description"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(testimonialCriteria["UserID"]));

            _cmd.Parameters.Add("@TestimonialID", SqlDbType.BigInt);
            _cmd.Parameters["@TestimonialID"].Direction = ParameterDirection.Output;

            result = objUtility.ExecuteScalar(_cmd);
            return result;
        }
        #endregion

        #region To get specific Testimonial details
        public TestimonialModel GetTestimonialSpecificDetails(Hashtable testimonialCriteria)
        {
            var _db = new DBUtility();
            var temp = new TestimonialModel();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetTestimonialSpecificDetails";
            _cmd.Parameters.AddWithValue("@TestimonialID", Convert.ToInt32(testimonialCriteria["ID"]));

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Author = Convert.ToString(dr["Author"]);
                    temp.Description = Convert.ToString(dr["Description"]);
                    temp.ImagePath = Convert.ToString(dr["ImagePath"]);
                    temp.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
            }
            return temp;
        }
        #endregion

        #region Update Testimonial Image Path
        public long UpdateTestimonialImagePath(Hashtable testimonialCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_UpdateTestimonialImagePath";

            if (string.IsNullOrWhiteSpace(Convert.ToString(testimonialCriteria["Filename"])))
            {
                _cmd.Parameters.AddWithValue("@TestimonialImagepath", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@TestimonialImagepath", Convert.ToString(testimonialCriteria["Filename"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@TestimonialID", Convert.ToInt32(testimonialCriteria["ID"]));

            result = objUtility.ExecuteScalar(_cmd);
            return result;
        }
        #endregion

        #region Delete current Testimonial details
        public int DeleteCurrentTestimonial(Hashtable testimonialCriteria)
        {
            var _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_DeleteCurrentTestimonial";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(testimonialCriteria["UserID"]));
            if (Convert.ToInt32(testimonialCriteria["ID"]) <= 0)
            {
                _cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(testimonialCriteria["ID"]));
            }

            return _db.ExecuteNonQuery(_cmd);
        }
        #endregion

    }
}