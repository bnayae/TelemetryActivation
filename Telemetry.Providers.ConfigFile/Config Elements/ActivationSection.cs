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
        #region MinImportance

        [ConfigurationProperty("min-importance",
            DefaultValue = nameof(ImportanceLevel.Normal),
            IsRequired = false)]
        //[RegexStringValidator("(Low|Normal|High)")]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public ImportanceLevel MinImportance
        {
            get
            {
                object val = this["min-importance"];
                if (val == null)
                    return 0;
                return (ImportanceLevel)val;
            }
            set
            {
                this["min-importance"] = value;
            }
        }

        #endregion // MinImportance

        #region Limits

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public LimitToCollection Limits
        {
            get
            {
                var hostCollection = (LimitToCollection)base[""];
                return hostCollection;
            }
        }

        #endregion // Limits

        #region DebugView

        internal class DebugView
        {
            private ActivationSection _instance;

            public DebugView(ActivationSection instance)
            {
                this._instance = instance;
            }


            public ImportanceLevel MinImportance => _instance.MinImportance;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public LimitToConfigElement[] Limits
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<LimitToConfigElement> Get()
                    {
                        foreach (var exclude in _instance.Limits)
                        {
                            yield return (LimitToConfigElement)exclude;
                        }
                    }
                }
            }
        }

        #endregion // DebugView
    }
}
