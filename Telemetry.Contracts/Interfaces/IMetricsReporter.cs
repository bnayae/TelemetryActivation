using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporter: IDisposable
    {
        /// <summary>
        /// Report quantity at point in time.
        /// Classic usage is for reporting operation starts
        /// from this kind of report you can understand the throughput over the time-line.
        /// </summary>
        /// <param name="importance">The importance.</param>
        /// <param name="tags">The tags.</param>
        void Count(
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    IReadOnlyDictionary<string, string> tags = null);

        /// <summary>
        /// Report operation duration.
        /// </summary>
        /// <param name="importance">The importance.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        IDisposable Duration(
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    IReadOnlyDictionary<string, string> tags = null);
    }
}
