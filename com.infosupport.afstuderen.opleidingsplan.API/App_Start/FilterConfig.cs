using System.Web;
using System.Web.Mvc;

namespace com.infosupport.afstuderen.opleidingsplan.api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
