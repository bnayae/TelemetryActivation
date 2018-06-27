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
    public class ActivationSection : ActivationInfo
    {
        private const string METRIC_THRESHOLD = "metric-threshold";
        private const string TEXTUAL_THRESHOLD = "textual-threshold";

        #region MetricThreshold

        [ConfigurationProperty(METRIC_THRESHOLD,
            DefaultValue = nameof(ImportanceLevel.Normal),
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
            DefaultValue = nameof(LogEventLevel.Information),
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

        #region Constricts

        [ConfigurationProperty("constricts", IsDefaultCollection = false)]
        public ConstrictCollection Constricts
        {
            get
            {
                var hostCollection = (ConstrictCollection)base["constricts"];
                return hostCollection;
            }
        }

        #endregion // Constricts

        #region Extends

        [ConfigurationProperty("extends", IsDefaultCollection = false)]
        public ExtendCollection Extends
        {
            get
            {
                var hostCollection = (ExtendCollection)base["extends"];
                return hostCollection;
            }
        }

        #endregion // Extends

        #region DebugView

        internal class DebugView
        {
            private ActivationSection _instance;

            public DebugView(ActivationSection instance)
            {
                this._instance = instance;
            }


            public ImportanceLevel MetricThreshold => _instance.MetricThreshold;
            public LogEventLevel TextualThreshold => _instance.TextualThreshold;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ConfigItemElement[] Constricts
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<ConfigItemElement> Get()
                    {
                        foreach (var exclude in _instance.Constricts)
                        {
                            yield return (ConfigItemElement)exclude;
                        }
                    }
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ConfigItemElement[] Extends
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<ConfigItemElement> Get()
                    {
                        foreach (var extend in _instance.Extends)
                        {
                            yield return (ConfigItemElement)extend;
                        }
                    }
                }
            }
        }

        #endregion // DebugView
    }
}
