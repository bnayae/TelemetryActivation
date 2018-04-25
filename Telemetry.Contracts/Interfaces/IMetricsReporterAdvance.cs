using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface IMetricsReporterAdvance: IDisposable
    {
        void Report(
                    IReadOnlyDictionary<string, object> fields = null,
                    IReadOnlyDictionary<string, string> tags = null,
                    ImportanceLevel importance = ImportanceLevel.Normal);
        void Count(
                 ImportanceLevel importance = ImportanceLevel.Normal,
                 IReadOnlyDictionary<string, string> tags = null,
                 int count = 1);

        IDisposable Duration(
                    ImportanceLevel importance = ImportanceLevel.Normal,
                    IReadOnlyDictionary<string, string> tags = null);
    }
}
