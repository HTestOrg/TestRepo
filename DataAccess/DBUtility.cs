using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class DBUtility
    {
        #region Common variables
        readonly SqlConnection _con = new SqlConnection();
        private readonly SqlCommand _cmd;
        readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        #endregion

        public DBUtility()
        {
            try
            {
                _con.ConnectionString = _connectionString;
                _cmd = _con.CreateCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Connection
        {
            get { return _con.ConnectionString = _connectionString; }
            set { }
        }

        private void OpenConnection()
        {
            if (_con.State == ConnectionState.Closed || _con.State == ConnectionState.Broken)
            {
                _con.Open();
            }
        }

        public void CloseConnection()
        {
            _con.Close();
        }

        public DataSet FillDataSet(SqlCommand pcmd, DataSet ds)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            pcmd.Connection = _con;

            try
            {
                da.SelectCommand = pcmd;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _cmd.Dispose();
                da.Dispose();
            }
            return ds;
        }

        public bool ExecuteCommand(SqlCommand pcmd)
        {
            bool Result = false;
            SqlDataAdapter da = new SqlDataAdapter();
            OpenConnection();
            pcmd.Connection = _con;
            try
            {
                SqlDataReader dr = pcmd.ExecuteReader();
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                throw ex;
            }
            finally
            {
                CloseConnection();
                _cmd.Dispose();
                da.Dispose();
            }
            return Result;
        }

        public long ExecuteScalar(SqlCommand pcmd)
        {
            int Result = 0;
            OpenConnection();
            pcmd.Connection = _con;
            try
            {
                Result = Convert.ToInt32(pcmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Result = 0;
                throw ex;
            }
            finally
            {
                _cmd.Dispose();
            }
            return Result;
        }

        public int ExecuteNonQuery(SqlCommand pcmd)
        {
            int Result = 0;
            OpenConnection();
            pcmd.Connection = _con;
            try
            {
                Result = Convert.ToInt32(pcmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                Result = 0;
                throw ex;
            }
            finally
            {
                _cmd.Dispose();
            }
            return Result;
        }

        public DataTable FillDataTable(SqlCommand pcmd, DataTable dt)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            pcmd.Connection = _con;
            try
            {
                da.SelectCommand = pcmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _cmd.Dispose();
                da.Dispose();
            }
            return dt;
        }
        
    }

    static class DataRowExtensions
    {
        public static object GetValue( this DataRow row, string column )
        {
            return row.Table.Columns.Contains( column ) ? row[ column ] : null;
        }
    }
}