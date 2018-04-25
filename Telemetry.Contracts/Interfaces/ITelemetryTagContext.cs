using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ITelemetryTagContext: ITelemetryPushContext
    {
        #region Tags

        /// <summary>
        /// Gets the tokens.
        /// </summary>
        ImmutableDictionary<string, string> Tags { get; }

        #endregion // Tags
    }
}
