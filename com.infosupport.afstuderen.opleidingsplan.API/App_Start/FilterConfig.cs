using System.Web;
using System.Web.Mvc;

namespace com.infosupport.afstuderen.opleidingsplan.api
{
    public sealed class FilterConfig
    {
        private FilterConfig() {}
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
