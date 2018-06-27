using Contracts;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telemetry.Implementation
{
    public class LogFactory : ILogFactory
    {
        //public static void SetLogFactory(
        //    LoggerConfiguration setting,
        //    ITelemetryActivation activation)
        //{
        //    Current = new LogFactory(setting, activation);
        //}
        //public static ILogFactory Current { get; private set; }

        private readonly ILogger _logger;
        public LogFactory(
            LoggerConfiguration setting,
            ITelemetryActivation activation)
        {
            // will be handle by the activation
            var minLevel = activation.TextualThreshold < LogEventLevel.Debug ? activation.TextualThreshold : LogEventLevel.Debug;
            setting = setting
                    .MinimumLevel.Is(minLevel)
                    .Filter.ByIncludingOnly(l => activation.IsActive(l.Level))
                    .Enrich.With<Enrichment>();

            _logger = setting.CreateLogger();
            //Log.Logger = _logger;
        }

        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <returns></returns>
        public ILogger Create() => _logger;
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public ILogger Create<T>(T instance) => Create<T>();
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ILogger Create<T>() => _logger.ForContext<T>();
    }
}
