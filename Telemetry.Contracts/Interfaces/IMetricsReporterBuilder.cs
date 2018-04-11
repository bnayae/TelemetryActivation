using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterBuilder
    {
        IMetricsReporterBuilder AddTag<T>(string tagName, T tagValue);
        IMetricsReporter ForContext<T>();
        IMetricsReporter ForContext(string componentTag);
    }
}
