using System.Web.Mvc;
using Synoptek.Filters;

namespace Synoptek
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add( new ExceptionHandlingAttribute());
        }
    }
}