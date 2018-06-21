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
    [DebuggerDisplay("{MetricThreshold}, {TextualThreshold}")]
    public struct ActivationLevel //: IEquatable<ActivationLevel>
    {
        #region Ctor

        public ActivationLevel(
            ImportanceLevel metricThreshold,
            LogEventLevel textualThreshold)
        {
            MetricThreshold = metricThreshold;
            TextualThreshold = textualThreshold;
        }

        #endregion // Ctor

        #region MetricThreshold

        public ImportanceLevel MetricThreshold { get; }

        #endregion // MetricThreshold

        #region TextualThreshold

        public LogEventLevel TextualThreshold { get; }

        #endregion // TextualThreshold

        //#region Equality Pattern

        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as ActivationLevel);
        //}

        //public bool Equals(ActivationLevel other)
        //{
        //    return other != null &&
        //           MetricThreshold == other.MetricThreshold &&
        //           TextualThreshold == other.TextualThreshold;
        //}

        //public override int GetHashCode()
        //{
        //    var hashCode = -1095595053;
        //    hashCode = hashCode * -1521134295 + MetricThreshold.GetHashCode();

        //    return hashCode;
        //}

        //public static bool operator ==(ActivationLevel element1, ActivationLevel element2)
        //{
        //    return EqualityComparer<ActivationLevel>.Default.Equals(element1, element2);
        //}

        //public static bool operator !=(ActivationLevel element1, ActivationLevel element2)
        //{
        //    return !(element1 == element2);
        //}

        //#endregion // Equality Pattern
    }
}
