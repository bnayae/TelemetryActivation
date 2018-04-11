using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts;
using InfluxDB.Collector;

namespace Telemetry.Implementation
{
    public class MetricsReporter :
        IMetricsReporter,
        IMetricsReporterAdvance
    {
        private readonly IMetricsConfig _config;
        private readonly string _measurementName;
        private readonly IImmutableDictionary<string, string> _tags;
        private readonly MetricsCollector _influxClient;

        public MetricsReporter(
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

        void IMetricsReporter.Count(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags, 
            [CallerMemberName]
            string operationName = null)
        {
            tags = _tags.Add("OperationName", operationName)
                        .AddRange(tags);

            if (_config.IsActive(importance, tags))
                _influxClient.Increment(_measurementName, tags: tags);
        }

        void IMetricsReporterAdvance.Count(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags,
            int count,
            [CallerMemberName]
            string operationName = null)
        {
            tags = _tags.Add("OperationName", operationName)
                        .AddRange(tags);

            if (_config.IsActive(importance, tags))
                _influxClient.Increment(_measurementName, count, tags: tags);
        }

        public IDisposable Duration(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags,
            [CallerMemberName]
            string operationName = null)
        {
            tags = _tags.Add("OperationName", operationName)
                        .AddRange(tags);

            IDisposable result = NonDisposable.Default;
            if (_config.IsActive(importance, tags))
                result = _influxClient.Time(_measurementName, tags: tags);
            return result;
        }

        void IMetricsReporterAdvance.Report(
            IReadOnlyDictionary<string, object> fields,
            IReadOnlyDictionary<string, string> tags,
            ImportanceLevel importance,
            [CallerMemberName]
            string operationName = null)
        {
            tags = _tags.Add("OperationName", operationName)
                        .AddRange(tags);

            if (_config.IsActive(importance, tags))
                _influxClient.Write(_measurementName, fields, tags);
        }

        private class NonDisposable : IDisposable
        {
            public readonly static IDisposable Default = new NonDisposable();
            public void Dispose()
            {
            }
        }
    }
}
