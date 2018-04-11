using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporter
    {
        void Count(
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    IReadOnlyDictionary<string, string> tags = null,
                    [CallerMemberName]string operationName = null);

        IDisposable Duration(
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    IReadOnlyDictionary<string, string> tags = null,
                    [CallerMemberName]string operationName = null);
    }
}
