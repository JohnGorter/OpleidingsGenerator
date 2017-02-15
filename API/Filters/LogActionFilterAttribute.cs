using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Filters
{
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private static ILog _logger = LogManager.GetLogger(typeof(LogActionFilterAttribute));
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = actionContext.ActionDescriptor.ActionName;
     
            var lines = actionContext.ActionArguments.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            var arguments = string.Join(",", lines);

            _logger.Info(string.Format(_culture, "OnActionExecuting: {0} {1} arguments: {2}", controllerName, actionName, arguments));
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            _logger.Info(string.Format(_culture, "OnActionExecuted: {0} {1}", controllerName, actionName));
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}