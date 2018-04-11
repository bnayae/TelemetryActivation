using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ITelemetryActivation
    {
        bool IsActive(
            ImportanceLevel metricLevel,
            string ComponentTag = null);
    }
}
