using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ITelemetryActivationContext: ITelemetryPushContext
    {
        #region Tokens

        /// <summary>
        /// Gets the tokens.
        /// </summary>
        ImmutableHashSet<string> Tokens { get; }

        #endregion // Tokens

        #region HasToken

        bool HasToken(string token);

        #endregion // HasToken
    }
}
