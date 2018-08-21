using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using System.Collections;

namespace DataAccess.DataAccess
{
    public class LoginDA
    {
        #region Common Variables
        private SqlCommand _cmd;
        #endregion

        #region login user
        public LoginModel LoginUser(Hashtable loginCriteria)
        {
            DBUtility _db = new DBUtility();
            var token = new LoginModel();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_UserLogin";
            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["UserName"])))
            {
                _cmd.Parameters.AddWithValue("@UserName", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserName", Convert.ToString(loginCriteria["UserName"]).Trim());
            }
            _dt = _db.FillDataTable( _cmd, _dt );
            if( _dt.Rows.Count > 0 )
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var common = new Common();
                    common.UserId = Convert.ToString(dr["UserID"]);
                    common.EmailAddress = Convert.ToString(dr["EmailAddress"]);
                    common.UserName = Convert.ToString(dr["UserName"]);
                    common.RoleType = Convert.ToString(dr["RoleType"]);
                    common.IsSubscribed = Convert.ToString(dr["IsSubscribed"]);
                    common.IsEnabled = Convert.ToBoolean(dr["IsEnabled"]);
                    common.Password = Convert.ToString(dr["Password"]);
                    token.common = common;
                }
            }
            return token;
        }
        #endregion

        #region Get User Details
        public UserDetailsModel GetUserDetails(Hashtable loginCriteria)
        {
            DBUtility _db = new DBUtility();
            _cmd = new SqlCommand();
            var _dt = new DataTable();
            var result = new UserDetailsModel();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetUserDetailsByToken";
            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["Token"])))
            {
                _cmd.Parameters.AddWithValue("@Token", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Token", Convert.ToString(loginCriteria["Token"]).Trim());
            }
            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new UserDetailsModel();
                    temp.UserId = Convert.ToString(dr["UserID"]);
                    temp.UserName = Convert.ToString(dr["UserName"]);
                    temp.Email = Convert.ToString(dr["Email"]);
                    result = temp;
                }
            }
            return result;
        }
        #endregion

        #region Validation Security Questions
        public bool ValidationSecQuestions(Hashtable loginCriteria)
        {
            DBUtility _db = new DBUtility();
            _cmd = new SqlCommand();
            var _dt = new DataTable();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_ValidationSecQuestions";

            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["UserID"])))
            {
                _cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(loginCriteria["UserID"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@PrimarySecurityQuestionId", Convert.ToInt32(loginCriteria["PrimarySecurityQuestionId"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["PrimarySecurityQuestionAnswer"])))
            {
                _cmd.Parameters.AddWithValue("@PrimarySecurityQuestionAnswer", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@PrimarySecurityQuestionAnswer", Convert.ToString(loginCriteria["PrimarySecurityQuestionAnswer"]).Trim());
            }

            _cmd.Parameters.AddWithValue("@SecondarySecurityQuestionId", Convert.ToInt32(loginCriteria["SecondarySecurityQuestionId"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["SecondarySecurityQuestionAnswer"])))
            {
                _cmd.Parameters.AddWithValue("@SecondarySecurityQuestionAnswer", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SecondarySecurityQuestionAnswer", Convert.ToString(loginCriteria["SecondarySecurityQuestionAnswer"]).Trim());
            }

            var result = _db.ExecuteScalar(_cmd);
            if (result > 0)
                return true;

            return false;
        }
        #endregion

        #region Change Password
        public bool ChangePassword(Hashtable loginCriteria)
        {
            DBUtility _db = new DBUtility();
            _cmd = new SqlCommand();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_ChangeUserPassword";

            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["Email"])))
            {
                _cmd.Parameters.AddWithValue("@Email", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Email", Convert.ToString(loginCriteria["Email"]).Trim());
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(loginCriteria["Password"])))
            {
                _cmd.Parameters.AddWithValue("@Password", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Password", Convert.ToString(loginCriteria["Password"]).Trim());
            }

            var result = _db.ExecuteScalar(_cmd);
            if (result > 0)
                return true;
            return false;
        }
        #endregion
    }
}