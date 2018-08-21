using System.Web.Mvc;
using System.Web.Routing;

namespace Synoptek.Filters
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Synoptek.SessionController.UserSession.UserId == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Login" },  
                                          { "action", "LogOff" }  
                                         });
            }

            base.OnActionExecuting(filterContext);
        }

    }
}