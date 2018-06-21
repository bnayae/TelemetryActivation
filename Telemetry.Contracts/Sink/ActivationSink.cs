using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Serilog
{
    public static class ActivationSinkExtensions
    {
        public static LoggerConfiguration WithActivation(
            this LoggerSinkConfiguration sinkConfiguration, 
            ITelemetryActivation activation,
            string channelKey,
            Action<LoggerSinkConfiguration> wrappedSinkConfigure)
        {
            return LoggerSinkConfiguration.Wrap(
                            sinkConfiguration,
                            s => new ActivationSink(channelKey, s, activation),
                            wrappedSinkConfigure,
                            LogEventLevel.Verbose,
                            levelSwitch: null);
        }
    }

    public class ActivationSink : ILogEventSink
    {
        private readonly ILogEventSink _decoratedSink;
        private readonly string _channelKey;
        private readonly ITelemetryActivation _activation;

        public ActivationSink(
            string channelKey,
            ILogEventSink decoratedSink,
            ITelemetryActivation activation)
        {
            _decoratedSink = decoratedSink;
            _channelKey = channelKey;
            _activation = activation;
        }

        public void Emit(LogEvent logEvent)
        {
            if (!_activation.IsActive(logEvent.Level, _channelKey))
                return;
            _decoratedSink.Emit(logEvent);
        }
    }

}
