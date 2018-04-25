using Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DebuggerDisplay("{Path}")]
    public class ActivationFilter : IEquatable<ActivationFilter>
    {
        #region Ctor


        public ActivationFilter(string path)
        {
            Path = path;
        }

        #endregion // Ctor

        #region Path

        public string Path { get; }

        #endregion // Path

        #region Equality Pattern

        public override bool Equals(object obj)
        {
            return Equals(obj as ActivationFilter);
        }

        public bool Equals(ActivationFilter other)
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

        public static bool operator ==(ActivationFilter element1, ActivationFilter element2)
        {
            return EqualityComparer<ActivationFilter>.Default.Equals(element1, element2);
        }

        public static bool operator !=(ActivationFilter element1, ActivationFilter element2)
        {
            return !(element1 == element2);
        }

        #endregion // Equality Pattern
    }
}
