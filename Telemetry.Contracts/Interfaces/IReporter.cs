using Contracts;

namespace Contracts
{
    public interface IReporter
    {
        ILogFactory LogFactory { get; }
        IMetricsReporter Metric { get; }
        ITelemetryPushContext PushContext { get; }
    }
}