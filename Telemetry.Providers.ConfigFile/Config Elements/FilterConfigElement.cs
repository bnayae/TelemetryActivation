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
    public class FilterConfigElement : ConfigurationElement, IEquatable<FilterConfigElement>
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

        #region Equality Pattern

        public override bool Equals(object obj)
        {
            return Equals(obj as FilterConfigElement);
        }

        public bool Equals(FilterConfigElement other)
        {
            return other != null &&
                   Path == other.Path;
        }

        public override int GetHashCode()
        {
            var hashCode = -345578410;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            return hashCode;
        }

        public static bool operator ==(FilterConfigElement element1, FilterConfigElement element2)
        {
            return EqualityComparer<FilterConfigElement>.Default.Equals(element1, element2);
        }

        public static bool operator !=(FilterConfigElement element1, FilterConfigElement element2)
        {
            return !(element1 == element2);
        }

        #endregion // Equality Pattern
    }
}
