using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Serilog.Events;

namespace Contracts
{
    /// <summary>
    /// Contract for telemetry activation
    /// </summary>
    public interface ITelemetryActivation
    {
        ImportanceLevel MetricThreshold { get; }

        LogEventLevel TextualThreshold { get; }

        /// <summary>
        /// Determines whether the specified metric level is active.
        /// </summary>
        /// <param name="metricLevel">The metric level.</param>
        /// <returns>
        ///   <c>true</c> if the specified metric level is active; otherwise, <c>false</c>.
        /// </returns>
        bool IsActive(ImportanceLevel metricLevel);

        /// <summary>
        /// Determines whether the specified log level is active.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>
        ///   <c>true</c> if the specified log level is active; otherwise, <c>false</c>.
        /// </returns>
        bool IsActive(LogEventLevel level);
    }
}
