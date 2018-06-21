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
    public class ActivationSetting : ActivationUnit
    {
        #region Ctor

        public ActivationSetting(
            ImportanceLevel metricThreshold,
            LogEventLevel textualThreshold,
            IEnumerable<ActivationItem> constricts,
            IEnumerable<ActivationItem> extends,
            IEnumerable<KeyValuePair<string, ActivationUnit>> channels)
            : base(/*metricThreshold, textualThreshold, */constricts, extends)
        {
            Threshold = new ActivationThreshold(metricThreshold, textualThreshold);
            Channels = new ConcurrentDictionary<string, ActivationUnit>(channels);
        }

        #endregion // Ctor

        #region Threshold
        public ActivationThreshold Threshold { get; }
        #endregion // Threshold

        #region Channels

        /// <summary>
        /// Gets the channels.
        /// </summary>
        public ConcurrentDictionary<string, ActivationUnit> Channels { get; }

        #endregion // Channels

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

            public IDictionary<string, ActivationUnit> Channels => _instance.Channels;
        }

        #endregion // DebugView
    }
}
