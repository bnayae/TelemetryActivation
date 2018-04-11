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
    public class ConfigProvider : IMetricsConfig
    {
        private readonly ActivationSection _config;
        public ConfigProvider()
        {
            _config =
                ConfigurationManager.GetSection("activationSection") as ActivationSection;
        }

        public bool IsActive(ImportanceLevel level, 
            IReadOnlyDictionary<string, string> tags)
        {
            if (!_config.Enable)
                return false;
            //if (level < _config.Excludes.any.MinImportance)
            //    return false;
            return true;

            //foreach (var tag in tags)
            //{
            //    // TODO: index it
            //    if(_config.Exclude.Tags.an)
            //}
        }

        #region DebugView

        internal class DebugView
        {
            private ConfigProvider _instance;

            public DebugView(ConfigProvider instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationSection ConfigSection => _instance._config;
        }

        #endregion // DebugView
    }
}
