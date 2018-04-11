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


            public ImportanceLevel MinImportance => _instance.MinImportance;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ConstrictConfigElement[] Constricts
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<ConstrictConfigElement> Get()
                    {
                        foreach (var exclude in _instance.Constricts)
                        {
                            yield return (ConstrictConfigElement)exclude;
                        }
                    }
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ExtendConfigElement[] Extends
            {
                get
                {
                    return Get().ToArray();
                    IEnumerable<ExtendConfigElement> Get()
                    {
                        foreach (var extend in _instance.Extends)
                        {
                            yield return (ExtendConfigElement)extend;
                        }
                    }
                }
            }
        }

        #endregion // DebugView
    }
}
