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
    [DebuggerDisplay("{Threshold.Metric}, {Threshold.Textual}")]
    public class ActivationItem //: IEquatable<ActivationItem>
    {
        #region Ctor

        public ActivationItem(
            ImportanceLevel metricThreshold,
            LogEventLevel textualThreshold,
            IEnumerable<ActivationFilter> filters)
        {
            Threshold = new ActivationThreshold(metricThreshold, textualThreshold);
            Filters = filters?.ToArray() ?? Array.Empty<ActivationFilter>();
        }

        #endregion // Ctor

        #region Threshold

        public ActivationThreshold Threshold { get; }

        #endregion // Threshold

        public int GetThreshold(TelemetryActivationKind kind)
        {
            switch (kind)
            {
                case TelemetryActivationKind.Metric:
                    return (int)Threshold.Metric;
                case TelemetryActivationKind.Textual:
                    return (int)Threshold.Textual;
            }
            throw new ArgumentOutOfRangeException($"Invalid kind {kind}");
        }

        #region Filters

        public ActivationFilter[] Filters { get; set; }

        #endregion // Filters

        #region DebugView

        internal class DebugView
        {
            private ActivationItem _instance;

            public DebugView(ActivationItem instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MetricThreshold => _instance.Threshold.Metric;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public LogEventLevel TextualThreshold => _instance.Threshold.Textual;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationFilter[] Filters => _instance.Filters.ToArray();
        }

        #endregion // DebugView

        //#region Equality Pattern

        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as ActivationItem);
        //}

        //public bool Equals(ActivationItem other)
        //{
        //    return other != null &&
        //           MetricThreshold == other.MetricThreshold &&
        //           EqualityComparer<ActivationFilter[]>.Default.Equals(Filters, other.Filters);
        //}

        //public override int GetHashCode()
        //{
        //    var hashCode = -1095595053;
        //    hashCode = hashCode * -1521134295 + MetricThreshold.GetHashCode();
        //    foreach (var filter in Filters)
        //    {
        //        hashCode = hashCode * -1521134295 + filter.GetHashCode();
        //    }

        //    return hashCode;
        //}

        //public static bool operator ==(ActivationItem element1, ActivationItem element2)
        //{
        //    return EqualityComparer<ActivationItem>.Default.Equals(element1, element2);
        //}

        //public static bool operator !=(ActivationItem element1, ActivationItem element2)
        //{
        //    return !(element1 == element2);
        //}

        //#endregion // Equality Pattern
    }
}
