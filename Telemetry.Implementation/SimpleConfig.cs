using Contracts;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telemetry.Implementation
{
    public class SimpleConfig : ISimpleConfig
    {
        public string this[string key] => 
            ConfigurationManager.AppSettings[key];
    }
}
