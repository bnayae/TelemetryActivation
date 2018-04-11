using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterAdvanceBuilder 
    {
        IMetricsReporterAdvanceBuilder AddTag<T>(string tagName, T tagValue);
        IMetricsReporterAdvance ForContext<T>();
        IMetricsReporterAdvance ForContext(string componentTag);
        IMetricsReporterAdvanceBuilder Measurement(string measurementName);
    }
}
