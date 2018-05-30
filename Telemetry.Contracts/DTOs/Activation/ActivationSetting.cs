using Contracts;
using Serilog.Events;
using System;
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
    public class ActivationSetting
    {
        #region Ctor

        public ActivationSetting(
            ImportanceLevel metricThreshold,
            LogEventLevel textualThreshold,
            IEnumerable<ActivationItem> constricts,
            IEnumerable<ActivationItem> extends)
        {
            MetricThreshold = metricThreshold;
            TextualThreshold = textualThreshold;
            Constricts = constricts?.ToArray() ?? Array.Empty<ActivationItem>();
            Extends = extends?.ToArray() ?? Array.Empty<ActivationItem>();
        }

        #endregion // Ctor

        #region MetricThreshold

        public ImportanceLevel MetricThreshold { get; }

        #endregion // MetricThreshold

        #region TextualThreshold

        public LogEventLevel TextualThreshold { get; }

        #endregion // TextualThreshold

        #region Constricts

        public ActivationItem[] Constricts { get; }

        #endregion // Constricts

        #region Extends
        public ActivationItem[] Extends { get; }

        #endregion // Extends

        #region DebugView

        internal class DebugView
        {
            private ActivationSetting _instance;

            public DebugView(ActivationSetting instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MetricThreshold => _instance.MetricThreshold;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public LogEventLevel TextualThreshold => _instance.TextualThreshold;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationItem[] Constricts => _instance.Constricts.ToArray();

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationItem[] Extends => _instance.Extends.ToArray();
        }

        #endregion // DebugView
    }
}
