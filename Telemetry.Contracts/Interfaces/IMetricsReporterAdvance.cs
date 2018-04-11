using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterAdvance
    {
        void Report(
                    IReadOnlyDictionary<string, object> fields = null,
                    IReadOnlyDictionary<string, string> tags = null,
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    [CallerMemberName]
                    string operationName = null);
        void Count(
                 ImportanceLevel importance = ImportanceLevel.Normal,
                 IReadOnlyDictionary<string, string> tags = null,
                 int count = 1,
                 [CallerMemberName]string operationName = null);

        IDisposable Duration(
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    IReadOnlyDictionary<string, string> tags = null,
                    [CallerMemberName]string operationName = null);
    }
}
