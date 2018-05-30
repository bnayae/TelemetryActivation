using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterAdvanceBuilder 
    {
        /// <summary>
        /// Override the current or default measurement name.
        /// </summary>
        /// <param name="measurementName">Name of the measurement.</param>
        /// <returns></returns>
        IMetricsReporterBuilder Measurement(string measurementName);

        /// <summary>
        /// Create the actual reporter (according to the builder state).
        /// </summary>
        /// <returns></returns>
        IMetricsReporterAdvance Build();
    }
}
