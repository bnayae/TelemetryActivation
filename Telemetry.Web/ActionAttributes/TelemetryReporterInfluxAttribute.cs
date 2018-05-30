﻿using Contracts;
using Serilog;
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
        private static LogFactory _logFactory;


        public TelemetryReporterInfluxAttribute()
        {
            IMetricsReporterBuilder builder = new MetricsReporterBuilder(
                 _activationFactory,
                 _simpleConfig,
                 _tagContext);
            _reporter = builder.Build();

            var logConfig = new LoggerConfiguration();
            logConfig = logConfig
                            .WriteTo.File("log.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .WriteTo.Seq("http://localhost:5341");
            var activation = _activationFactory.Create();
            _logFactory = new LogFactory(logConfig, activation);
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
            var reflected = actionContext?.ActionDescriptor as ReflectedHttpActionDescriptor;
            var className = reflected?.MethodInfo?.ReflectedType?.Name ?? "Unknown";
            _activationContext.PushFlow(CommonLayerOrService.WebApi, className, actionName);

            _reporter.Count(ImportanceLevel.Normal);
            var operation = _reporter.Duration(ImportanceLevel.Normal);
            actionContext.ActionArguments.Add("end-action", operation);
            var logger = _logFactory.Create();//.ForContext()

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