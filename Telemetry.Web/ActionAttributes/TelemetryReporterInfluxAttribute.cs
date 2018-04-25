using Contracts;
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
        private readonly ITelemetryActivationContext _activationContext = TelemetryActivationContext.Default;
        private readonly ITelemetryTagContext _tagContext = TelemetryTagContext.Default;
        private readonly ISimpleConfig _simpleConfig = new SimpleConfig();
        private readonly ITelemetryPushContext _telemetryPushContext =
                        new TelemetryPushContext(
                                            TelemetryActivationContext.Default,
                                            TelemetryTagContext.Default);
        private readonly ITelemetryActivationFactory _activationFactory =
                                new ConfigActivationProvider(TelemetryActivationContext.Default);

        //private static ObjectCache _cache = MemoryCache.Default;
        private const string COMPONENT_NAME = "webapi";
        private static IMetricsReporter _reporter;

        public TelemetryReporterInfluxAttribute()
        {
            IMetricsReporterBuilder builder = new MetricsReporterBuilder(
                 _activationFactory,
                 _simpleConfig,
                 _tagContext);
            _reporter = builder.Build();
        }

        public override void OnActionExecuting(
            HttpActionContext actionContext)
        {
            //actionContext.RequestContext.Principal.Identity.Name
            var request = actionContext.Request;
            string actionName = request.GetActionDescriptor().ActionName;


            //InfluxManager.Default.TryAddTagRange(tags);
            _tagContext.PushToken("method", request.Method.Method);
            _tagContext.PushToken("uri", request.Method.Method);
            _telemetryPushContext.PushToken("request-version", request.Method.Method);
            _activationContext.PushFlow(CommonLayerOrService.WebApi, "Unknown", actionName);

            _reporter.Count(ImportanceLevel.Normal);
            var operation = _reporter.Duration(ImportanceLevel.Normal);
            actionContext.ActionArguments.Add("end-action", operation);
        }

        public override void OnActionExecuted(
            HttpActionExecutedContext actionExecutedContext)
        {
            var operation = (IDisposable)actionExecutedContext.ActionContext.ActionArguments["end-action"];
            operation.Dispose();
        }

    }
}