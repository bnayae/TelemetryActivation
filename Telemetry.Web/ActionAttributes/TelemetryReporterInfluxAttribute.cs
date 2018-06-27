﻿using Contracts;
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
        private static readonly ITelemetryActivationContext _activationContext = TelemetryActivationContext.Default;
        private static readonly ITelemetryTagContext _tagContext = TelemetryTagContext.Default;
        private static readonly ISimpleConfig _simpleConfig = new SimpleConfig();
        private static readonly ITelemetryPushContext _telemetryPushContext =
                        new TelemetryPushContext(
                                            TelemetryActivationContext.Default,
                                            TelemetryTagContext.Default);
        private static readonly ITelemetryActivationFactory _activationFactory =
                                new ConfigActivationProvider(TelemetryActivationContext.Default);

        //private static ObjectCache _cache = MemoryCache.Default;
        private const string COMPONENT_NAME = "webapi";
        private static IMetricsReporter _reporter;
        private static ILogFactory _logFactory;


        static TelemetryReporterInfluxAttribute()
        {
            IMetricsReporterBuilder builder = new MetricsReporterBuilder(
                 _activationFactory,
                 _simpleConfig,
                 _tagContext);
            _reporter = builder.Build();

            var activation = _activationFactory.Create();
            var logConfig = new LoggerConfiguration();
            logConfig = logConfig
                            .MinimumLevel.Verbose()
                            .WriteTo.File(
                                    "log.debug.txt",
                                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .WriteTo.File(
                                    "log.error.txt",
                                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .WriteTo.File(
                                    "log.warn.txt",
                                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                            ;
                            //.WriteTo.WithActivation(
                            //    activation, "seq",
                            //    s => s.Seq("http://localhost:5341",restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug));
            LogFactory.SetLogFactory(logConfig, activation);
            _logFactory = LogFactory.Current;
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