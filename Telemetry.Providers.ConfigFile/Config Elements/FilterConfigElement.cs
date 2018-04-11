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
    [DebuggerDisplay("{Path}")]
    public class FilterConfigElement : ConfigurationElement
    {
        #region Ctor

        public FilterConfigElement()
        {
        }

        public FilterConfigElement(string path)
        {
            Path = path;
        }

        #endregion // Ctor

        #region Path

        [ConfigurationProperty("path", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        #endregion // Path
    }
}
