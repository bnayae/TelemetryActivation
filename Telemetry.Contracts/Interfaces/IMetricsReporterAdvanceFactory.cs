using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterAdvanceFactory 
    {
        IMetricsReporterAdvanceFactory AddTag<T>(string tagName, T tagValue);
        IMetricsReporterAdvance ForContext<T>();
        IMetricsReporterAdvance ForContext(string contextName);
        IMetricsReporterAdvanceFactory Measurement(string measurementName);
    }
}
