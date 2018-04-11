using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsConfig
    {
        bool IsActive(
            ImportanceLevel level,
            IReadOnlyDictionary<string, string> tags);
    }
}
