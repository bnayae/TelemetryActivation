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
    public class ChannelConfigItemElement: ActivationInfo
    {
        private const string KEY = "key";
     
        #region Key

        [ConfigurationProperty(KEY, IsRequired = true)]
        //[RegexStringValidator("(Low|Normal|High)")]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public string Key
        {
            get
            {
                object val = this[KEY];
                if (val == null)
                    throw new NotImplementedException("key is required");
                return (string)val;
            }
            set
            {
                this[KEY] = value;
            }
        }

        #endregion // Key

        #region DebugView

        internal class DebugView
        {
            private ChannelConfigItemElement _instance;

            public DebugView(ChannelConfigItemElement instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public string Key => _instance.Key;


            //public ImportanceLevel MetricThreshold => _instance.MetricThreshold;
            //public LogEventLevel TextualThreshold => _instance.TextualThreshold;

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
