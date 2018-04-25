using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterBuilder
    {
        /// <summary>
        /// Adds the tag (db level tags).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="tagValue">The tag value.</param>
        /// <returns></returns>
        IMetricsReporterBuilder AddTag<T>(string tagName, T tagValue);

        /// <summary>
        /// Append component information
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layerOrService">The layer or service.</param>
        /// <param name="alternativeLayerOrService">
        /// When layerOrService == Other you can specify alternative layerOrService name.
        /// </param>    
        /// <returns></returns>
        IMetricsReporterBuilder AddComponentInfo<T>(
            CommonLayerOrService layerOrService = CommonLayerOrService.Other,
            string alternativeLayerOrService = null);

        /// <summary>
        /// Adds the component information.
        /// </summary>
        /// <param name="layerOrService">The layer or service.</param>
        /// <param name="layerOrServiceClassName">Name of the layer or service class.</param>
        /// <param name="alternativeLayerOrService">
        /// When layerOrService == Other you can specify alternative layerOrService name.
        /// </param>    
        /// <returns></returns>
        IMetricsReporterBuilder AddComponentInfo(
            CommonLayerOrService layerOrService = CommonLayerOrService.Other,
            string layerOrServiceClassName = null,
            string alternativeLayerOrService = null);

        IMetricsReporterAdvanceBuilder Advance { get; }

        /// <summary>
        /// Create the actual reporter (according to the builder state).
        /// </summary>
        /// <returns></returns>
        IMetricsReporter Build();
    }
}
