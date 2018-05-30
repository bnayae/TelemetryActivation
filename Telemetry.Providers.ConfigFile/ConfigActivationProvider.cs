using Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Providers.ConfigFile
{
    public class ConfigActivationProvider : ITelemetryActivationFactory
    {
        private readonly TelemetryActivation _activation;

        #region Ctor

        public ConfigActivationProvider(
            ITelemetryActivationContext activationContext)
        {
            var config =
                ConfigurationManager.GetSection("activationSection") as ActivationSection;
            var setting =
                new ActivationSetting(
                            config.MetricThreshold,
                            config.TextualThreshold,
                            config.Constricts,
                            config.Extends);
            _activation = new TelemetryActivation(setting, activationContext);
        }

        #endregion // Ctor

        public ITelemetryActivation Create() => _activation;
    }
}
