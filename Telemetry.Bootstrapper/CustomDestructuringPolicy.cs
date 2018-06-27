using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;

namespace Telemetry.Bootstrapper
{
    public class CustomDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(
            object value, 
            ILogEventPropertyValueFactory propertyValueFactory, 
            out LogEventPropertyValue result)
        {
            result = propertyValueFactory.CreatePropertyValue(value, true);
            return true;
        }
    }
}
