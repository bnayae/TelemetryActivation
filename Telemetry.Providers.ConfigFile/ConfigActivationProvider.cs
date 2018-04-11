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
    [DebuggerTypeProxy(typeof(DebugView))]
    public class ConfigActivationProvider : ITelemetryActivationFactory
    {
        private readonly TelemetryActivation _activation;
        private readonly ActivationSetting _setting; // keep for the debugger
        public ConfigActivationProvider()
        {
            var config =
                ConfigurationManager.GetSection("activationSection") as ActivationSection;
            _setting =
                new ActivationSetting(
                            config.MinImportance,
                            config.Constricts,
                            config.Extends);
            _activation = new TelemetryActivation(_setting);
        }

        public ITelemetryActivation Create() => _activation;

        #region DebugView

        internal class DebugView
        {
            private ConfigActivationProvider _instance;

            public DebugView(ConfigActivationProvider instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationSetting ConfigSection => _instance._setting;
        }

        #endregion // DebugView
    }
}
