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
    public sealed class MetricsReporter :
        IMetricsReporter,
        IMetricsReporterAdvance
    {
        private readonly ITelemetryActivation _activation;
        private readonly string _measurementName;
        private readonly IImmutableDictionary<string, string> _tags;
        private readonly MetricsCollector _influxClient;
        private readonly string _componentTag;

        #region Ctor

        public MetricsReporter(
            ITelemetryActivation activation,
            string measurementName,
            IImmutableDictionary<string, string> tags,
            MetricsCollector influxClient,
            string componentTag)
        {
            _activation = activation;
            _measurementName = measurementName;
            _tags = tags;
            _influxClient = influxClient;
            _componentTag = componentTag;
        }

        #endregion // Ctor

        #region Count

        public void Count(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags,
            [CallerMemberName]
            string operationName = null)
        {
            if (_activation.IsActive(importance, _componentTag))
            {
                tags = _tags.Add("OperationName", operationName)
                            .AddRange(tags);
                _influxClient.Increment(_measurementName, tags: tags);
            }
        }

        public void Count(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags,
            int count,
            [CallerMemberName]
            string operationName = null)
        {
            if (_activation.IsActive(importance, _componentTag))
            {
                tags = _tags.Add("OperationName", operationName)
                      .AddRange(tags);
                _influxClient.Increment(_measurementName, count, tags: tags);
            }
        }

        #endregion // Count

        #region Duration

        public IDisposable Duration(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags,
            [CallerMemberName]
            string operationName = null)
        {
            IDisposable result = NonDisposable.Default;
            if (_activation.IsActive(importance, _componentTag))
            {
                tags = _tags.Add("OperationName", operationName)
                            .AddRange(tags);

                result = _influxClient.Time(_measurementName, tags: tags);
            }
            return result;
        }

        public void Report(
            IReadOnlyDictionary<string, object> fields,
            IReadOnlyDictionary<string, string> tags,
            ImportanceLevel importance,
            [CallerMemberName]
            string operationName = null)
        {
            if (_activation.IsActive(importance, _componentTag))
            {
                tags = _tags.Add("OperationName", operationName)
                            .AddRange(tags);

                _influxClient.Write(_measurementName, fields, tags);
            }
        }

        #endregion // Duration

        #region Dispose Pattern

        public void Dispose()
        {
            _influxClient.Dispose();
            GC.SuppressFinalize(this);
        }

        ~MetricsReporter() => Dispose();

        #endregion // Dispose Pattern

        #region NonDisposable [nested]

        private class NonDisposable : IDisposable
        {
            public readonly static IDisposable Default = new NonDisposable();
            public void Dispose()
            {
            }
        }

        #endregion // NonDisposable [nested]
    }
}
