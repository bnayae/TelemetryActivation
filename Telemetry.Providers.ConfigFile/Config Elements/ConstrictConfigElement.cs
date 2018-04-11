using Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://msdn.microsoft.com/en-us/library/system.diagnostics.debuggertypeproxyattribute(v=vs.110).aspx

namespace Telemetry.Providers.ConfigFile
{
    [DebuggerTypeProxy(typeof(DebugView))]
    [DebuggerDisplay("Constrict: {Importance}")]
    public class ConstrictConfigElement : ConfigurationElement, IEquatable<ConstrictConfigElement>
    {
        #region Importance

        [ConfigurationProperty("importance",
            DefaultValue = nameof(ImportanceLevel.Normal),
            IsRequired = false)]
        //[RegexStringValidator("(Low|Normal|High)")]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public ImportanceLevel Importance
        {
            get
            {
                object val = this["importance"];
                if (val == null)
                    return 0;
                return (ImportanceLevel)val;
            }
            set
            {
                this["importance"] = value;
            }
        }

        #endregion // Importance

        #region ComponentTag

        [ConfigurationProperty("component-tag",
            DefaultValue = nameof(ImportanceLevel.Normal),
            IsRequired = false)]
        //[RegexStringValidator("(Low|Normal|High)")]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public string ComponentTag
        {
            get
            {
                object val = this["component-tag"];
                return val?.ToString() ?? string.Empty;
            }
            set
            {
                this["component-tag"] = value;
            }
        }

        #endregion // ComponentTag

        #region Filters

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public FilterCollection Filters
        {
            get { return (FilterCollection)base[""]; }
        }

        #endregion // Filters

        #region DebugView

        internal class DebugView
        {
            private ConstrictConfigElement _instance;

            public DebugView(ConstrictConfigElement instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MinImportance => _instance.Importance;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public FilterConfigElement[] Filters
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<FilterConfigElement> Get()
                    {
                        foreach (var filter in _instance.Filters)
                        {
                            yield return (FilterConfigElement)filter;
                        }
                    }
                }
            }
        }

        #endregion // DebugView

        #region Equality Pattern

        public override bool Equals(object obj)
        {
            return Equals(obj as ConstrictConfigElement);
        }

        public bool Equals(ConstrictConfigElement other)
        {
            return other != null &&
                   Importance == other.Importance &&
                   ComponentTag == other.ComponentTag &&
                   EqualityComparer<FilterCollection>.Default.Equals(Filters, other.Filters);
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

        public static bool operator ==(ConstrictConfigElement element1, ConstrictConfigElement element2)
        {
            return EqualityComparer<ConstrictConfigElement>.Default.Equals(element1, element2);
        }

        public static bool operator !=(ConstrictConfigElement element1, ConstrictConfigElement element2)
        {
            return !(element1 == element2);
        }

        #endregion // Equality Pattern
    }
}
