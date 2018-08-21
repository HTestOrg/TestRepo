using BusinessObjects;
using System;
using System.IO;
using System.Configuration;

namespace ExceptionHandling
{
    public class Logger
    {
        #region Handle exception
        public CustomAppException HandleException(string funcName, Exception exception, string userID)
        {
            CustomAppException customException = new CustomAppException();
            customException.ErrorSource = funcName;
            customException.ErrorMessage = exception.Message;
            customException.StackTrace = exception.StackTrace;
            return HandleCustomException(funcName, customException, userID);
        }
        #endregion

        #region Handle custom exception
        public CustomAppException HandleCustomException(string funcName, CustomAppException customException, string userID)
        {
            CustomAppException ex = new CustomAppException();
            ex.ErrorMessage = customException.ErrorMessage;
            ex.ErrorSource = customException.ErrorSource;
            ex.StackTrace = customException.StackTrace;
            LogException(customException, userID);
            return ex;
        }
        #endregion

        #region log exception in text file
        public bool LogException(CustomAppException customizedException, string userID)
        {
            System.IO.StreamWriter objStreamWriter = null;
            bool status = false;
            try
            {
                string filename = DateTime.Today.ToString("yyyyMMdd");
                string ErrorLogPath = null;

                ErrorLogPath = GetPath(filename);

                if (!Directory.Exists(ErrorLogPath))
                {
                    Directory.CreateDirectory(ErrorLogPath);
                }
                var currentDateTime = DateTime.Now;
                string errorFilename = currentDateTime.ToString("ddMMyyyy _hh_mm_ss");

                objStreamWriter = new System.IO.StreamWriter(ErrorLogPath + errorFilename + ".txt", true);

                objStreamWriter.WriteLine("UserID: " + userID);
                objStreamWriter.WriteLine("ErrorSource : " + customizedException.ErrorSource);
                objStreamWriter.WriteLine("ErrorMessage: " + customizedException.ErrorMessage);
                objStreamWriter.WriteLine("StackTrace: " + customizedException.StackTrace);

                status = true;
            }
            finally
            {
                objStreamWriter.Flush();
                objStreamWriter.Close();
                objStreamWriter.Dispose();
            }

            return status;
        }
        #endregion

        #region Get path details
        public string GetPath(string date)
        {
            string path = string.Empty;
            string pathReadFromConfig = string.Empty;
            string folderName = string.Empty;

            folderName = date;

            pathReadFromConfig = ConfigurationManager.AppSettings["ErrorLogPath"].ToString();
            path = pathReadFromConfig + '\\' + folderName + '\\';

            return path;
        }
        #endregion
    }
}
