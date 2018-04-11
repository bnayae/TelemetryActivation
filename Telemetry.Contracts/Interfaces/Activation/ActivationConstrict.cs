using Contracts;
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
    [DebuggerDisplay("Constrict: {Importance}")]
    public class ActivationConstrict : IEquatable<ActivationConstrict>
    {
        #region Ctor

        public ActivationConstrict(
            ImportanceLevel importance,
            string componentTag,
            IEnumerable<ActivationFilter> filters)
        {
            Importance = importance;
            ComponentTag = componentTag;
            Filters = filters?.ToArray() ?? Array.Empty<ActivationFilter>();
        }

        #endregion // Ctor

        #region Importance

        public ImportanceLevel Importance { get; }

        #endregion // Importance

        #region ComponentTag

        public string ComponentTag { get; }

        #endregion // ComponentTag

        #region Filters

        public ActivationFilter[] Filters { get; set; }

        #endregion // Filters

        #region DebugView

        internal class DebugView
        {
            private ActivationConstrict _instance;

            public DebugView(ActivationConstrict instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MinImportance => _instance.Importance;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationFilter[] Filters => _instance.Filters.ToArray();
        }

        #endregion // DebugView

        #region Equality Pattern

        public override bool Equals(object obj)
        {
            return Equals(obj as ActivationConstrict);
        }

        public bool Equals(ActivationConstrict other)
        {
            return other != null &&
                   Importance == other.Importance &&
                   ComponentTag == other.ComponentTag &&
                   EqualityComparer<ActivationFilter[]>.Default.Equals(Filters, other.Filters);
        }

        public override int GetHashCode()
        {
            var hashCode = -1095595053;
            hashCode = hashCode * -1521134295 + Importance.GetHashCode();
            hashCode = hashCode * -1521134295 + ComponentTag.GetHashCode();
            foreach (var filter in Filters)
            {
                hashCode = hashCode * -1521134295 + filter.GetHashCode();
            }

            return hashCode;
        }

        public static bool operator ==(ActivationConstrict element1, ActivationConstrict element2)
        {
            return EqualityComparer<ActivationConstrict>.Default.Equals(element1, element2);
        }

        public static bool operator !=(ActivationConstrict element1, ActivationConstrict element2)
        {
            return !(element1 == element2);
        }

        #endregion // Equality Pattern
    }
}
