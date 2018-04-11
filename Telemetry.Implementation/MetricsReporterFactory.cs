using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using InfluxDB.Collector;

namespace Telemetry.Implementation
{
    public class MetricsReporterFactory :
        IMetricsReporterFactory,
        IMetricsReporterAdvanceFactory
    {
        private readonly IMetricsConfig _config;
        private readonly string _measurementName = "performance";
        private readonly IImmutableDictionary<string, string> _tags = ImmutableDictionary<string, string>.Empty;

        private static IMetricsReporterFactory Create(IMetricsConfig config) =>
                            new MetricsReporterFactory(config);
        private static IMetricsReporterAdvanceFactory CreateAdvance(IMetricsConfig config) =>
                            new MetricsReporterFactory(config);
        private readonly MetricsCollector _influxClient;

        public MetricsReporterFactory(IMetricsConfig config)
        {
            _config = config;
            _influxClient = new CollectorConfiguration()
                 .Tag.With("version", "v1")
                 .Tag.With("host", Environment.MachineName)
                 .Tag.With("user", Environment.UserName)
                 .Batch.AtInterval(TimeSpan.FromSeconds(5))
                 .WriteTo.InfluxDB("http://localhost:8086", database: "playground")
                 .CreateCollector();
        }

        private MetricsReporterFactory(
            IMetricsConfig config,
            string measurementName,
            IImmutableDictionary<string, string> tags,
            MetricsCollector influxClient)
        {
            _config = config;
            _measurementName = measurementName;
            _tags = tags;
            _influxClient = influxClient;
        }

        private MetricsReporterFactory AddTag<T>(string tagName, T tagValue)
        {
            var tags = _tags.Add(tagName, tagValue?.ToString());
            return new MetricsReporterFactory(_config, _measurementName, tags, _influxClient);
        }

        IMetricsReporterFactory IMetricsReporterFactory.AddTag<T>(string tagName, T tagValue)
        {
            return AddTag(tagName, tagValue);
        }

        IMetricsReporterAdvanceFactory IMetricsReporterAdvanceFactory.AddTag<T>(
            string tagName, T tagValue)
        {
            return AddTag(tagName, tagValue);
        }

        IMetricsReporter IMetricsReporterFactory.ForContext<T>()
        {
            var tags = _tags.Add("OperationGroup", typeof(T).Name);
            tags = tags.Add("Namespace", typeof(T).Namespace);
            return new MetricsReporter(_config, _measurementName, tags, _influxClient);
        }

        IMetricsReporterAdvance IMetricsReporterAdvanceFactory.ForContext<T>()
        {
            var tags = _tags.Add("OperationGroup", typeof(T).Name);
            tags = tags.Add("Namespace", typeof(T).Namespace);
            return new MetricsReporter(_config, _measurementName, tags, _influxClient);
        }

        IMetricsReporter IMetricsReporterFactory.ForContext(string contextName)
        {
            var tags = _tags.Add("OperationGroup", contextName);
            return new MetricsReporter(_config, _measurementName, tags, _influxClient);
        }

        IMetricsReporterAdvance IMetricsReporterAdvanceFactory.ForContext(string contextName)
        {
            var tags = _tags.Add("OperationGroup", contextName);
            return new MetricsReporter(_config, _measurementName, tags, _influxClient);
        }

        IMetricsReporterAdvanceFactory IMetricsReporterAdvanceFactory.Measurement(
                            string measurementName)
        {
            return new MetricsReporterFactory(_config, measurementName, _tags, _influxClient);
        }
    }
}
