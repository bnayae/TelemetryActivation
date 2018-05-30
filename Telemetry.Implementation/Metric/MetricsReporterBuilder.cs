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

        private readonly CollectorConfiguration _influxConfiguration;
        private readonly ITelemetryTagContext _tagContext;

        public IMetricsReporterAdvanceBuilder Advance => throw new NotImplementedException();

        #region Create

        /// <summary>
        /// Factory (create instance).
        /// The factory is here for backward compatibility, newer component should use IoC (Dependency Injection)
        /// </summary>
        /// <param name="activationFactory">The activation factory.</param>
        /// <param name="simpleConfig">The simple configuration.</param>
        /// <param name="tagContext">The tag context.</param>
        /// <returns></returns>
        private static IMetricsReporterBuilder Create(
            ITelemetryActivationFactory activationFactory,
            ISimpleConfig simpleConfig,
            ITelemetryTagContext tagContext)
        {
            return new MetricsReporterBuilder(activationFactory, simpleConfig, tagContext);
        }

        #endregion // Create

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsReporterBuilder"/> class.
        /// </summary>
        /// <param name="activationFactory">The activation factory.</param>
        /// <param name="simpleConfig">The simple configuration.</param>
        /// <param name="tagContext">The tag context.</param>
        public MetricsReporterBuilder(
            ITelemetryActivationFactory activationFactory,
            ISimpleConfig simpleConfig,
            ITelemetryTagContext tagContext)
        {
            // TODO: move it into Influx provider (InfluxMetricFactory)
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
            _tagContext = tagContext;
        }

        private MetricsReporterBuilder(
            ITelemetryActivation config,
            ITelemetryTagContext tagContext,
            string measurementName,
            IImmutableDictionary<string, string> tags,
            CollectorConfiguration influxConfiguration)
        {
            _activation = config;
            _tagContext = tagContext;
            _measurementName = measurementName;
            _tags = tags;
            _influxConfiguration = influxConfiguration;
        }

        #endregion // Ctor

        #region AddTag

        /// <summary>
        /// Adds the tag (db level tags).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="tagValue">The tag value.</param>
        /// <returns></returns>
        public IMetricsReporterBuilder AddTag<T>(string tagName, T tagValue)
        {
            var tags = _tags.Add(tagName, tagValue?.ToString());
            return new MetricsReporterBuilder(_activation, _tagContext, _measurementName, tags, _influxConfiguration);
        }

        #endregion // AddTag

        #region AddComponentInfo

        #region Overloads

        public IMetricsReporterBuilder AddComponentInfo<T>(
            CommonLayerOrService layerOrService = CommonLayerOrService.Other, 
            string alternativeLayerOrService = null)
        {
            return AddComponentInfo(layerOrService, typeof(T).Name, alternativeLayerOrService);
        }

        #endregion // Overloads

        public IMetricsReporterBuilder AddComponentInfo(
            CommonLayerOrService layerOrService = CommonLayerOrService.Other, 
            string layerOrServiceClassName = null, 
            string alternativeLayerOrService = null)
        {
            #region Validation 

            if (layerOrService != CommonLayerOrService.Other &&
                !string.IsNullOrEmpty(alternativeLayerOrService))
            {
                throw new ArgumentException($"{nameof(alternativeLayerOrService)} is not valid when {nameof(layerOrService)} != {nameof(CommonLayerOrService.Other)}");
            }

            #endregion // Validation

            #region string layer = ...

            string layer;
            if (layerOrService == CommonLayerOrService.Other)
                layer = alternativeLayerOrService;
            else
                layer = layerOrService.ToString();

            #endregion // string layer = ...

            var tags = _tags.Add("layer", layer);
            tags = tags.Add("component", layerOrServiceClassName);
            return new MetricsReporterBuilder(_activation, _tagContext, _measurementName, tags, _influxConfiguration);
        }

        #endregion // AddComponentInfo

        #region Build

        /// <summary>
        /// Create the actual reporter (according to the builder state).
        /// </summary>
        /// <returns></returns>
        IMetricsReporter IMetricsReporterBuilder.Build()
        {
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporter(_activation, _tagContext, _measurementName, _tags, influxClient);
        }

        /// <summary>
        /// Create the actual reporter (according to the builder state).
        /// </summary>
        /// <returns></returns>
        IMetricsReporterAdvance IMetricsReporterAdvanceBuilder.Build()
        {
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporter(_activation, _tagContext, _measurementName, _tags, influxClient);
        }

        #endregion // Build

        #region Measurement

        /// <summary>
        /// Override the current or default measurement name.
        /// </summary>
        /// <param name="measurementName">Name of the measurement.</param>
        /// <returns></returns>
        public IMetricsReporterBuilder Measurement(string measurementName)
        {
            var influxClient = _influxConfiguration.CreateCollector();
            return new MetricsReporterBuilder(_activation, _tagContext, measurementName, _tags, _influxConfiguration);
        }

        #endregion // Measurement
    }
}
