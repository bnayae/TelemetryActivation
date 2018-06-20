using Contracts;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Telemetry.Providers.ConfigFile
{
    public class TelemetryActivationSinkDecorator :
        ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}
