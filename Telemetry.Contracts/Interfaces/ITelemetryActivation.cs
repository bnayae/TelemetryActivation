using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    /// <summary>
    /// Contract for telemetry activation
    /// </summary>
    public interface ITelemetryActivation
    {
        /// <summary>
        /// Determines whether the specified metric level is active.
        /// </summary>
        /// <param name="metricLevel">The metric level.</param>
        /// <returns>
        ///   <c>true</c> if the specified metric level is active; otherwise, <c>false</c>.
        /// </returns>
        bool IsActive(
            ImportanceLevel metricLevel);
    }
}
