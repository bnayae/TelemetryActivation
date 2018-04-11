using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ITelemetryActivationFactory
    {
        ITelemetryActivation Create();
    }
}
