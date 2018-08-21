using System;
using System.Web.Mvc;
using NLog;
using Synoptek.Controllers;

namespace Synoptek.Filters
{
    public class ExceptionHandlingAttribute : HandleErrorAttribute
    {
        #region Common section
        Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Method to log the exception
        public override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            string userID = Convert.ToString(SessionController.UserSession.UserId);

            logger.Error(exception, "Error occured in Controller:" + controllerName + ", Action: " + actionName + ", User ID: " + userID + "");
            filterContext.ExceptionHandled = true;

            var controller = (ParentController)filterContext.Controller;
            if (controllerName == "Login")
            {
                //ToDo://Need to revise this change. If user had not visited to discourse yet. below page will load after logout
                if (actionName == "LogOff")
                {
                    filterContext.Result = controller.RedirectToAction("HomePage", "HomePage");
                }
                else
                {
                    filterContext.Result = controller.RedirectToAction("LoginError", "Login");
                }
            }
            else if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && SessionController.UserSession.UserId == null) {
                filterContext.Result = controller.RedirectToAction("LogOff", "Login");
            }
            else
            {
                filterContext.Result = controller.RedirectToAction("Error", "HomePage");
            }
        }
        #endregion
    }
}