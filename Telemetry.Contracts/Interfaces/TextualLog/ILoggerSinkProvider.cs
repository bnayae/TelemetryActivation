using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Contracts
{
    public interface ILoggerSinkProvider
    {
            // TODO: check ILogEventSink
           LoggerConfiguration Sink(LoggerSinkConfiguration config);
    }
}