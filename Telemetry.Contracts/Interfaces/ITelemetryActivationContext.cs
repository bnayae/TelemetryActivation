using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ITelemetryActivationContext
    {
        bool TryAppendToken<T>(T token);

        ImmutableHashSet<string> Tokens { get; }

        bool HasToken(string token);
    }
}
