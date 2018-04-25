using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using InfluxDB.Collector;

namespace Telemetry.Implementation
{
    public sealed class MetricsReporter :
        IMetricsReporter,
        IMetricsReporterAdvance
    {
        private readonly ITelemetryActivation _activation;
        private readonly ITelemetryTagContext _tagContext;
        private readonly string _measurementName;
        private readonly IImmutableDictionary<string, string> _tags;
        private readonly MetricsCollector _influxClient;

        #region Ctor

        public MetricsReporter(
            ITelemetryActivation activation,
            ITelemetryTagContext tagContext,
            string measurementName,
            IImmutableDictionary<string, string> tags,
            MetricsCollector influxClient)
        {
            _activation = activation;
            _tagContext = tagContext;
            _measurementName = measurementName;
            _tags = tags;
            _influxClient = influxClient;
        }

        #endregion // Ctor

        #region Count

        #region Overloads

        /// <summary>
        /// Report quantity at point in time.
        /// Classic usage is for reporting operation starts
        /// from this kind of report you can understand the throughput over the time-line.
        /// </summary>
        /// <param name="importance">The importance.</param>
        /// <param name="tags">The tags.</param>
        public void Count(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags)
        {
            Count(importance, tags, 1);
        }

        #endregion // Overloads

        /// <summary>
        /// Counts the specified importance.
        /// </summary>
        /// <param name="importance">The importance.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="count">The count.</param>
        public void Count(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags,
            int count)
        {
            if (_activation.IsActive(importance))
            {
                tags = _tags.AddRange(tags ?? ImmutableDictionary<string, string>.Empty)
                            .AddRange(_tagContext.Tags);
                _influxClient.Increment(_measurementName, count, tags: tags);
            }
        }

        #endregion // Count

        #region Duration

        /// <summary>
        /// Report operation duration.
        /// </summary>
        /// <param name="importance">The importance.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        public IDisposable Duration(
            ImportanceLevel importance,
            IReadOnlyDictionary<string, string> tags)
        {
            IDisposable result = NonDisposable.Default;
            if (_activation.IsActive(importance))
            {
                tags = _tags.AddRange(tags)
                            .AddRange(_tagContext.Tags);

                result = _influxClient.Time(_measurementName, tags: tags);
            }
            return result;
        }

        /// <summary>
        /// Reports the specified fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="importance">The importance.</param>
        public void Report(
            IReadOnlyDictionary<string, object> fields,
            IReadOnlyDictionary<string, string> tags,
            ImportanceLevel importance)
        {
            if (_activation.IsActive(importance))
            {
                var contextTags =
                tags = _tags.AddRange(tags)
                            .AddRange(_tagContext.Tags);

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
