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
    [DebuggerDisplay("Exclude: {MinImportance}")]
    public class ExcludeConfigElement : ConfigurationElement
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
            private ExcludeConfigElement _instance;

            public DebugView(ExcludeConfigElement instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ImportanceLevel MinImportance => _instance.MinImportance;

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
    }
}
