using Contracts;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://msdn.microsoft.com/en-us/library/system.diagnostics.debuggertypeproxyattribute(v=vs.110).aspx

namespace Contracts
{
    [DebuggerTypeProxy(typeof(DebugView))]
    [DebuggerDisplay("{Threshold.Metric}, {Threshold.Textual}")]
    public class ActivationSetting : ActivationUnit
    {
        #region Ctor

        public ActivationSetting(
            ImportanceLevel metricThreshold,
            LogEventLevel textualThreshold,
            IEnumerable<ActivationItem> constricts,
            IEnumerable<ActivationItem> extends)
            : base(/*metricThreshold, textualThreshold, */constricts, extends)
        {
            Threshold = new ActivationThreshold(metricThreshold, textualThreshold);
        }

        #endregion // Ctor

        #region Threshold
        public ActivationThreshold Threshold { get; }
        #endregion // Threshold

        #region DebugView

        internal class DebugView
        {
            private ActivationSetting _instance;

            public DebugView(ActivationSetting instance)
            {
                this._instance = instance;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MetricThreshold => _instance.Threshold.Metric;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public LogEventLevel TextualThreshold => _instance.Threshold.Textual;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationItem[] Constricts => _instance.Constricts.ToArray();

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationItem[] Extends => _instance.Extends.ToArray();
        }

        #endregion // DebugView
    }
}
