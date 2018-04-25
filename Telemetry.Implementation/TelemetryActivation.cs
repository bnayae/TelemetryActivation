using Contracts;
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

// TODO: ToLowwer()

namespace Telemetry.Providers.ConfigFile
{
    [DebuggerTypeProxy(typeof(DebugView))]
    public class TelemetryActivation :
        ITelemetryActivation
    {
        private readonly ActivationSetting _setting;
        private readonly ITelemetryActivationContext _activationContext;

        #region Ctor

        public TelemetryActivation(
            ActivationSetting setting,
            ITelemetryActivationContext activationContext)
        {
            _setting = setting;
            _activationContext = activationContext;
        }

        #endregion // Ctor

        #region IsActive

        /// <summary>
        /// Determines whether the specified metric level is active.
        /// </summary>
        /// <param name="metricLevel">The metric level.</param>
        /// <returns>
        ///   <c>true</c> if the specified metric level is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsActive(
            ImportanceLevel metricLevel)
        {
            if (metricLevel < _setting.MinImportance)
            {
                #region Check if pass the extends

                foreach (var extend in _setting.Extends)
                {
                    var settingImportance = extend.Importance;
                    if (settingImportance > metricLevel)
                        continue; // won't activate anyway

                    var tokenSupportAllFilters =
                        extend.Filters.All(m => _activationContext.HasToken(m.Path));
                    if (tokenSupportAllFilters)
                        return true; // both important and filters match
                }
                return false;

                #endregion // If(Extend) true
            }

            #region Check if constricted

            foreach (var constrict in _setting.Constricts)
            {
                var settingImportance = constrict.Importance;
                if (settingImportance < metricLevel)
                    continue; // activate anyway

                var tokenSupportAllFilters =
                    constrict.Filters.All(m => _activationContext.HasToken(m.Path));
                if (tokenSupportAllFilters)
                    return false; // both important and filters match
            }

            #endregion // Check if constricted
            return true;
        }

        #endregion // IsActive

        #region DebugView

        internal class DebugView
        {
            private TelemetryActivation _instance;

            public DebugView(TelemetryActivation instance)
            {
                this._instance = instance;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationSetting ConfigSection => _instance._setting;
        }

        #endregion // DebugView
    }
}
