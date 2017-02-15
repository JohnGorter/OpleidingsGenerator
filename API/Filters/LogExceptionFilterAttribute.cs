using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Filters
{
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static ILog _logger = LogManager.GetLogger(typeof(LogExceptionFilterAttribute));

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _logger.Error(actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}