using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterFactory
    {
        IMetricsReporterFactory AddTag<T>(string tagName, T tagValue);
        IMetricsReporter ForContext<T>();
        IMetricsReporter ForContext(string contextName);
    }
}
