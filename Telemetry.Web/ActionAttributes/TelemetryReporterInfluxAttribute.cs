using Contracts;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Telemetry.Implementation;
using Telemetry.Providers.ConfigFile;

namespace WebToInflux
{
    public class TelemetryReporterInfluxAttribute : ActionFilterAttribute
    {
        private static readonly IReporter _reporter = Reporter.Default;

        public override void OnActionExecuting(
            HttpActionContext actionContext)
        {
            //actionContext.RequestContext.Principal.Identity.Name
            var request = actionContext.Request;
            string actionName = request.GetActionDescriptor().ActionName;


            //InfluxManager.Default.TryAddTagRange(tags);
            _reporter.PushContext.PushToken("method", request.Method.Method);
            _reporter.PushContext.PushToken("uri", request.Method.Method);
            _reporter.PushContext.PushToken("request-version", request.Method.Method);
            var reflected = actionContext?.ActionDescriptor as ReflectedHttpActionDescriptor;
            var className = reflected?.MethodInfo?.ReflectedType?.Name ?? "Unknown";
            _reporter.PushContext.PushFlow(CommonLayerOrService.WebApi, className, actionName);

            _reporter.Metric.Count(ImportanceLevel.Normal);
            var operation = _reporter.Metric.Duration(ImportanceLevel.Normal);
            actionContext.ActionArguments.Add("end-action", operation);
            var logger = _reporter.LogFactory.Create();//.ForContext()

            logger.Information("Test {@url} {@host}", request.Method.Method, Environment.MachineName);
        }

        public override void OnActionExecuted(
            HttpActionExecutedContext actionExecutedContext)
        {
            var operation = (IDisposable)actionExecutedContext.ActionContext.ActionArguments["end-action"];
            operation.Dispose();
        }

    }
}