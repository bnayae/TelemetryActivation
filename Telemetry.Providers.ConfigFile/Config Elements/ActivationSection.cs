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
    public class ActivationSection : ConfigurationSection
    {
        #region Enable

        // Create a "remoteOnly" attribute.
        [ConfigurationProperty("enable",
            DefaultValue = "true",
            IsRequired = false)]
        public Boolean Enable
        {
            get
            {
                return (bool)this["enable"];
            }
            set
            {
                this["enable"] = value;
            }
        }

        #endregion // Enable

        #region Excludes

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ExcludeCollection Excludes
        {
            get
            {
                var hostCollection = (ExcludeCollection)base[""];
                return hostCollection;
            }
        }

        #endregion // Excludes

        #region DebugView

        internal class DebugView
        {
            private ActivationSection _instance;

            public DebugView(ActivationSection instance)
            {
                this._instance = instance;
            }


            //[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
            public Boolean Enable => _instance.Enable;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ExcludeConfigElement[] Excludes
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<ExcludeConfigElement> Get()
                    {
                        foreach (var exclude in _instance.Excludes)
                        {
                            yield return (ExcludeConfigElement)exclude;
                        }
                    }
                }
            }
        }

        #endregion // DebugView
    }
}
