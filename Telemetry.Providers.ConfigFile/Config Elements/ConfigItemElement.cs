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

namespace Telemetry.Providers.ConfigFile
{
    [DebuggerTypeProxy(typeof(DebugView))]
    [DebuggerDisplay("Constrict: {MetricThreshold}, {TextualThreshold}")]
    public class ConfigItemElement : ConfigurationElement, IEquatable<ConfigItemElement>
    {
        private const string METRIC_THRESHOLD = "metric-threshold";
        private const string TEXTUAL_THRESHOLD = "textual-threshold";
     
        #region MetricThreshold

        [ConfigurationProperty(METRIC_THRESHOLD,
            DefaultValue = null,
            IsRequired = false)]
        //[RegexStringValidator("(Low|Normal|High)")]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public ImportanceLevel MetricThreshold
        {
            get
            {
                object val = this[METRIC_THRESHOLD];
                if (val == null)
                    return 0;
                return (ImportanceLevel)val;
            }
            set
            {
                this[METRIC_THRESHOLD] = value;
            }
        }

        #endregion // MetricThreshold

        #region TextualThreshold

        [ConfigurationProperty(TEXTUAL_THRESHOLD,
            DefaultValue = null,
            IsRequired = false)]
        //[RegexStringValidator("(Verbose|Debug|Information|Warning|Error|Fatal)")]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public LogEventLevel TextualThreshold
        {
            get
            {
                object val = this[TEXTUAL_THRESHOLD];
                if (val == null)
                    return 0;
                return (LogEventLevel)val;
            }
            set
            {
                this[TEXTUAL_THRESHOLD] = value;
            }
        }

        #endregion // TextualThreshold

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
            private ConfigItemElement _instance;

            public DebugView(ConfigItemElement instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MetricThreshold => _instance.MetricThreshold;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public LogEventLevel TextualThreshold => _instance.TextualThreshold;

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

        public override bool Equals(object compareTo)
        {
            return Equals(compareTo as ConfigItemElement);
        }

        public bool Equals(ConfigItemElement other)
        {
            return other != null &&
                   MetricThreshold == other.MetricThreshold &&
                   TextualThreshold == other.TextualThreshold &&
                   EqualityComparer<FilterCollection>.Default.Equals(Filters, other.Filters);
        }

        public override int GetHashCode()
        {
            var hashCode = -1095595053;
            hashCode = hashCode * -1521134295 + MetricThreshold.GetHashCode();
            hashCode = hashCode * -1521134295 + TextualThreshold.GetHashCode();
            foreach (var filter in Filters)
            {
                hashCode = hashCode * -1521134295 + filter.GetHashCode();
            }

            return hashCode;
        }

        public static bool operator ==(ConfigItemElement element1, ConfigItemElement element2)
        {
            return EqualityComparer<ConfigItemElement>.Default.Equals(element1, element2);
        }

        public static bool operator !=(ConfigItemElement element1, ConfigItemElement element2)
        {
            return !(element1 == element2);
        }

        #endregion // Equality Pattern
    }
}
