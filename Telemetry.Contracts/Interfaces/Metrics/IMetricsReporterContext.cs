using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IMetricsReporterContext
    {                
        void PushTag<T>(
                    string tagName,
                    T tagValue);
    }
}
