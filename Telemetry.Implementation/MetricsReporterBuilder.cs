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
    public class MetricsReporterBuilder :
        IMetricsReporterBuilder,
        IMetricsReporterAdvanceBuilder
    {
        private readonly ITelemetryActivation _activation;
        private readonly string _measurementName = "performance";
        private readonly IImmutableDictionary<string, string> _tags = ImmutableDictionary<string, string>.Empty;

        private static IMetricsReporterBuilder Create(
            ITelemetryActivationFactory activationFactory,
            ISimpleConfig simpleConfig)
        {
            return new MetricsReporterBuilder(activationFactory, simpleConfig);
        }
        private static IMetricsReporterAdvanceBuilder CreateAdvance(
            ITelemetryActivationFactory activationFactory,
            ISimpleConfig simpleConfig)
        {
            return new MetricsReporterBuilder(activationFactory, simpleConfig);
        }

        private readonly CollectorConfiguration _influxConfiguration;

        #region Ctor

        public MetricsReporterBuilder(
            ITelemetryActivationFactory activationFactory,
            ISimpleConfig simpleConfig)
        {
            string url = simpleConfig["influx-url"];
            string db = simpleConfig["influx-database"];
            string version = simpleConfig["influx-report-version"];
            _activation = activationFactory.Create();
            _influxConfiguration = new CollectorConfiguration()
                 .Tag.With("version", version)
                 .Tag.With("host", Environment.MachineName)
                 .Tag.With("user", Environment.UserName)
                 .Batch.AtInterval(TimeSpan.FromSeconds(5))
                 .WriteTo.InfluxDB(url, database: db);
        }

        private MetricsReporterBuilder(
            ITelemetryActivation config,
            string measurementName,
            IImmutableDictionary<string, string> tags,
            CollectorConfiguration influxConfiguration)
        {
            _activation = config;
            _measurementName = measurementName;
            _tags = tags;
            _influxConfiguration = influxConfiguration;
        }

        #endregion // Ctor

        private MetricsReporterBuilder AddTag<T>(string tagName, T tagValue)
        {
            var tags = _tags.Add(tagName, tagValue?.ToString());
            return new MetricsReporterBuilder(_activation, _measurementName, tags, _influxConfiguration);
        }

        IMetricsReporterBuilder IMetricsReporterBuilder.AddTag<T>(string tagName, T tagValue)
        {
            return AddTag(tagName, tagValue);
        }

        IMetricsReporterAdvanceBuilder IMetricsReporterAdvanceBuilder.AddTag<T>(
            string tagName, T tagValue)
        {
            return AddTag(tagName, tagValue);
        }

        IMetricsReporter IMetricsReporterBuilder.ForContext<T>()
        {
            Type component = typeof(T);
            var tags = _tags.Add("ComponentTag", component.Name);
            tags = tags.Add("Namespace", typeof(T).Namespace);
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporter(_activation, _measurementName, tags, influxClient, component.FullName);
        }

        IMetricsReporterAdvance IMetricsReporterAdvanceBuilder.ForContext<T>()
        {
            Type component = typeof(T);
            var tags = _tags.Add("ComponentTag", component.Name);
            tags = tags.Add("Namespace", typeof(T).Namespace);
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporter(_activation, _measurementName, tags, influxClient, component.FullName);
        }

        IMetricsReporter IMetricsReporterBuilder.ForContext(string componentTag)
        {
            var tags = _tags.Add("ComponentTag", componentTag);
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporter(_activation, _measurementName, tags, influxClient, componentTag);
        }

        IMetricsReporterAdvance IMetricsReporterAdvanceBuilder.ForContext(string componentTag)
        {
            var tags = _tags.Add("ComponentTag", componentTag);
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporter(_activation, _measurementName, tags, influxClient, componentTag);
        }

        IMetricsReporterAdvanceBuilder IMetricsReporterAdvanceBuilder.Measurement(
                            string measurementName)
        {
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporterBuilder(_activation, measurementName, _tags, _influxConfiguration);
        }
    }
}
