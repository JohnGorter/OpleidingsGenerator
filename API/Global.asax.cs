using InfoSupport.KC.OpleidingsplanGenerator.Api;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Filters;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static ILog _logger = LogManager.GetLogger(typeof(WebApiApplication));

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfiguration.Configure();
            RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);
        }

        public static void RegisterWebApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new LogActionFilterAttribute());
            filters.Add(new LogExceptionFilterAttribute());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            _logger.Error("Application_Error", exc);
        }
    }
}
