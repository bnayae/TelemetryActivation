using Contracts;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telemetry.Implementation
{
    public class LogFactory: ILogFactory
    {
        private readonly ILogger _logger;
        public LogFactory(
            LoggerConfiguration setting,
            ITelemetryActivation activation)
        {
            setting = setting
                    .MinimumLevel.Is(activation.TextualThreshold)
                    .Filter.ByIncludingOnly(l => activation.IsActive(l.Level))
                    .Enrich.With<Enrichment>();

            _logger  = setting.CreateLogger();
            //Log.Logger = _logger;
        }

        public ILogger Create() => _logger;
    }
}
