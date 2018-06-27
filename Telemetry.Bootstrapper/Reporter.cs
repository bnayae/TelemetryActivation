using Contracts;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telemetry.Providers.ConfigFile;

namespace Telemetry.Implementation
{
    public class Reporter : IReporter
    {
        public static readonly IReporter Default = new Reporter();
        private const string FORMAT = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        //private readonly ITelemetryActivationContext _activationContext = TelemetryActivationContext.Default;
        //private readonly ITelemetryTagContext _tagContext = TelemetryTagContext.Default;
        private readonly ISimpleConfig _simpleConfig = new SimpleConfig();
        private readonly ITelemetryPushContext _telemetryPushContext =
                        new TelemetryPushContext(
                                            TelemetryActivationContext.Default,
                                            TelemetryTagContext.Default);
        private readonly ITelemetryActivationFactory _activationFactory =
                                new ConfigActivationProvider(TelemetryActivationContext.Default);

        //private static ObjectCache _cache = MemoryCache.Default;
        private const string COMPONENT_NAME = "webapi";

        public Reporter()
        {
            IMetricsReporterBuilder builder = new MetricsReporterBuilder(
                                 _activationFactory,
                                 _simpleConfig,
                                 TelemetryTagContext.Default);
            Metric = builder.Build();

            var activation = _activationFactory.Create();
            var logConfig = new LoggerConfiguration();
            logConfig = logConfig
                            .MinimumLevel.Verbose()
                            .WriteTo.File("log.debug.txt", restrictedToMinimumLevel: LogEventLevel.Debug, outputTemplate: FORMAT)
                            .WriteTo.File("log.error.txt", restrictedToMinimumLevel: LogEventLevel.Error, outputTemplate: FORMAT)
                            .WriteTo.File("log.warn.txt", restrictedToMinimumLevel: LogEventLevel.Warning, outputTemplate: FORMAT)
                            .WriteTo.Logger(l => l
                                .Filter.ByIncludingOnly(Matching.FromSource("WebToInfluxTake2.Controllers.DataController"))
                                .WriteTo.File("log.data.txt",outputTemplate: FORMAT))
                            ;
            //.WriteTo.WithActivation(
            //    activation, "seq",
            //    s => s.Seq("http://localhost:5341",restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug));

            LogFactory = new LogFactory(logConfig, activation);
            PushContext = _telemetryPushContext;
        }

        public ILogFactory LogFactory { get; }
        public ITelemetryPushContext PushContext { get; }
        public IMetricsReporter Metric { get; }
    }
}
