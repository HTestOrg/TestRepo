using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using System.Collections;


namespace DataAccess.DataAccess
{
    public class UserRegistrationDA
    {
        #region Common Variables
        private SqlCommand _cmd;

        #endregion

        #region Save data of User Registration
        public long SaveUserProfile(Hashtable userProfileCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_InsertUserProfile";

            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(userProfileCriteria["Name"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Email"])))
            {
                _cmd.Parameters.AddWithValue("@Email", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Email", Convert.ToString(userProfileCriteria["Email"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Address"])))
            {
                _cmd.Parameters.AddWithValue("@Address", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Address", Convert.ToString(userProfileCriteria["Address"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["CompanyName"])))
            {
                _cmd.Parameters.AddWithValue("@CompanyName", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@CompanyName", Convert.ToString(userProfileCriteria["CompanyName"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Password"])))
            {
                _cmd.Parameters.AddWithValue("@Password", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Password", Convert.ToString(userProfileCriteria["Password"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Gender"])))
            {
                _cmd.Parameters.AddWithValue("@Gender", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Gender", Convert.ToString(userProfileCriteria["Gender"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@RoleId", Convert.ToString(userProfileCriteria["RoleId"]));
            result = objUtility.ExecuteScalar(_cmd); 
            return result;
        }
        #endregion

        #region Update data of User Registration
        public long UpdateUserProfile(Hashtable userProfileCriteria)
        {
            long result = 0;
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_UpdateUserProfile";

            _cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(userProfileCriteria["ID"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(userProfileCriteria["Name"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Address"])))
            {
                _cmd.Parameters.AddWithValue("@Address", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Address", Convert.ToString(userProfileCriteria["Address"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["City"])))
            {
                _cmd.Parameters.AddWithValue("@City", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@City", Convert.ToString(userProfileCriteria["City"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["StateName"])))
            {
                _cmd.Parameters.AddWithValue("@StateName", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@StateName", Convert.ToString(userProfileCriteria["StateName"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["ZipCode"])))
            {
                _cmd.Parameters.AddWithValue("@ZipCode", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ZipCode", Convert.ToInt32(userProfileCriteria["ZipCode"]));
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["CompanyName"])))
            {
                _cmd.Parameters.AddWithValue("@CompanyName", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@CompanyName", Convert.ToString(userProfileCriteria["CompanyName"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["LicenceNumber"])))
            {
                _cmd.Parameters.AddWithValue("@LicenceNumber", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@LicenceNumber", Convert.ToString(userProfileCriteria["LicenceNumber"]));
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["PhoneNumber"])))
            {
                _cmd.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@PhoneNumber", Convert.ToString(userProfileCriteria["PhoneNumber"]));
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Password"])))
            {
                _cmd.Parameters.AddWithValue("@Password", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Password", Convert.ToString(userProfileCriteria["Password"]));
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["IsEnabled"])))
            {
                _cmd.Parameters.AddWithValue("@IsEnabled", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@IsEnabled", Convert.ToBoolean(userProfileCriteria["IsEnabled"]));
            }


            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(userProfileCriteria["UserID"]));

            result = objUtility.ExecuteScalar(_cmd);

            return result;
        }
        #endregion

        #region Get user Roles
        public List<UserRole> GetUserRoles()
        {
            List<UserRole> lstUserRoles = new List<UserRole>();

            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetUserRoles";

            var _dt = new DataTable();

            _dt = objUtility.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    UserRole objUser = new UserRole();
                    objUser.Id = Convert.ToInt32(dr["RoleID"]);

                    objUser.Name = Convert.ToString(dr["RoleName"]);
                    lstUserRoles.Add(objUser);
                }
            }
            return lstUserRoles;
        }
        #endregion

        #region Check If User Is Already Exist or Not
        public bool CheckIfUserIsAlredayExistOrNot(Hashtable userProfileCriteria)
        {
            DBUtility objUtility = new DBUtility();
            var _dt = new DataTable();
            bool result = false;
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_CheckIfUserIsAlreadyExist";
            _cmd.Parameters.AddWithValue("@Email", Convert.ToString(userProfileCriteria["Email"]).Trim());

            _dt = objUtility.FillDataTable(_cmd, _dt);

            if (_dt.Rows.Count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Check If User Enter the Correct Password or not
        public bool CheckIfUserEnterCorrectPassword(Hashtable userProfileCriteria)
        {
            DBUtility objUtility = new DBUtility();
            var _dt = new DataTable();
            bool result = false;
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_CheckIfUserEnterCorrectPassword";
            _cmd.Parameters.AddWithValue("@Password", Convert.ToString(userProfileCriteria["Password"]).Trim());

            _dt = objUtility.FillDataTable(_cmd, _dt);

            if (_dt.Rows.Count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Get user specific details

        public List<UserProfileEditModel> GetUserSpecificDetails(Hashtable userProfileCriteria)
        {
            var _db = new DBUtility();
            var token = new List<UserProfileEditModel>();
            var _dt = new DataTable();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetUserSpecificDetails";

            _cmd.Parameters.AddWithValue("@UserId", Convert.ToString(userProfileCriteria["UserID"]));

            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["Name"])))
            {
                _cmd.Parameters.AddWithValue("@Name", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@Name", Convert.ToString(userProfileCriteria["Name"]).Trim());
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["SortingOrder"])))
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@SortingOrder", Convert.ToString(userProfileCriteria["SortingOrder"]).Trim());
            }

            _dt = _db.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    var temp = new UserProfileEditModel();
                    temp.ID = Convert.ToInt32(dr["ID"]);
                    temp.Name = Convert.ToString(dr["Name"]);
                    temp.Email = Convert.ToString(dr["EmailAddress"]);
                    temp.Address = Convert.ToString(dr["Address"]);
                    temp.City = Convert.ToString(dr["City"]);
                    temp.StateName = Convert.ToString(dr["StateName"]);
                    temp.ZipCode = Convert.ToInt32(dr["ZipCode"]);
                    temp.CompanyName = Convert.ToString(dr["CompanyName"]);
                    temp.LicenceNumber = Convert.ToString(dr["LicenseNumber"]);
                    temp.PhoneNumber = Convert.ToString(dr["PhoneNumber"]);
                    temp.RoleId = Convert.ToInt32(dr["RoleID"]);
                    temp.IsPaid = Convert.ToBoolean(DataRowExtensions.GetValue(dr, "IsPaid"));
                    temp.CustomerID = Convert.ToString(DataRowExtensions.GetValue(dr, "CustomerID"));
                    temp.ProfileImage = Convert.ToString(dr["ProfileImage"]);
                    temp.IsEnabled = Convert.ToBoolean(DataRowExtensions.GetValue(dr, "IsEnabled"));
                    temp.Gender = Convert.ToString(dr["Gender"]);
                    temp.CreatedDate = Convert.ToDateTime(DataRowExtensions.GetValue(dr, "CreatedDate"));
                    temp.ModifiedDate = Convert.ToDateTime(DataRowExtensions.GetValue(dr, "ModifiedDate"));
                    temp.Password = Convert.ToString(DataRowExtensions.GetValue(dr, "Password"));
                    temp.RoleName = Convert.ToString(DataRowExtensions.GetValue(dr, "RoleName")); 
                    token.Add(temp);
                }
            }
            return token;
        }
        #endregion

        #region Get listing loan types
        public List<SecurityQuestion> GetSecurityQuestionList()
        {
            List<SecurityQuestion> lstSecurityQuestion = new List<SecurityQuestion>();

            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_GetSecurityQuestions";

            var _dt = new DataTable();

            _dt = objUtility.FillDataTable(_cmd, _dt);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    SecurityQuestion objSecurityQuestion = new SecurityQuestion();
                    objSecurityQuestion.Id = Convert.ToInt32(dr["ID"]);
                    objSecurityQuestion.Name = Convert.ToString(dr["Name"]);
                    lstSecurityQuestion.Add(objSecurityQuestion);
                }
            }
            return lstSecurityQuestion;
        }
        #endregion

        #region Update Profile Image Path
        public string UpdateProfileImagePath(Hashtable userProfileCriteria)
        {
            string result = "";
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_UpdateProfileImagePath";

            if (string.IsNullOrWhiteSpace(Convert.ToString(userProfileCriteria["ProfileImagePath"])))
            {
                _cmd.Parameters.AddWithValue("@ProfileImagePath", DBNull.Value);
            }
            else
            {
                _cmd.Parameters.AddWithValue("@ProfileImagePath", Convert.ToString(userProfileCriteria["ProfileImagePath"]).Trim());
            }
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(userProfileCriteria["UserID"]));
            result = objUtility.ExecuteScalar(_cmd).ToString();
            return result;
        }
        #endregion

        #region Update User Role
        public string UpdateUserRole(Hashtable userProfileCriteria)
        {
            string result = "";
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_UpdateUserRole";

            _cmd.Parameters.AddWithValue("@RoleID", Convert.ToInt64(userProfileCriteria["RoleID"]));
            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(userProfileCriteria["UserID"]));

            result = objUtility.ExecuteScalar(_cmd).ToString();
            return result;
        }
        #endregion

        #region Delete User
        public string DeleteUser(Hashtable userProfileCriteria)
        {
            string result = "";
            DBUtility objUtility = new DBUtility();
            _cmd = new SqlCommand();

            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "GP_SP_DeleteUser";

            _cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(userProfileCriteria["UserID"]));
            _cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(userProfileCriteria["ID"]));

            result = objUtility.ExecuteScalar(_cmd).ToString();
            return result;
        }
        #endregion
    }
}
