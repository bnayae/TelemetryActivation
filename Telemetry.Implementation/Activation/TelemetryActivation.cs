using Contracts;
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

        public ImportanceLevel MetricThreshold => _setting.MetricThreshold;

        public LogEventLevel TextualThreshold => _setting.TextualThreshold;

        #region IsActive

        /// <summary>
        /// Determines whether the specified metric level is active.
        /// </summary>
        /// <param name="metricLevel">The metric level.</param>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        ///   <c>true</c> if the specified metric level is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsActive(
                ImportanceLevel metricLevel,
                string channelKey = null)
        {
            if (!TryGetSettingUnit(channelKey, out ActivationUnit setting))
                return true; // the root level filtering should dictate the result when channel level is empty 

            if (metricLevel < setting.MetricThreshold)
            {
                #region Check if pass the extends

                foreach (var extend in setting.Extends)
                {
                    var settingImportance = extend.MetricThreshold;
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

            foreach (var constrict in setting.Constricts)
            {
                var settingImportance = constrict.MetricThreshold;
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

        /// <summary>
        /// Determines whether the specified log level is active.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// <c>true</c> if the specified log level is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsActive(
                LogEventLevel level,
                string channelKey = null)
        {
            if (!TryGetSettingUnit(channelKey, out ActivationUnit setting))
                return true; // the root level filtering should dictate the result when channel level is empty 

            if (level < setting.TextualThreshold)
            {
                #region Check if pass the extends

                foreach (var extend in setting.Extends)
                {
                    var settingImportance = extend.TextualThreshold;
                    if (settingImportance >= level)
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

            foreach (var constrict in setting.Constricts)
            {
                var settingImportance = constrict.TextualThreshold;
                if (settingImportance <= level)
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

        #region TryGetSettingUnit

        /// <summary>
        /// Gets the setting unit (root or channel level).
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="setting">The setting.</param>
        /// <returns>false when channel key != null and missing from configuration</returns>
        private bool TryGetSettingUnit(
                string channelKey,
                out ActivationUnit setting)
        {
            setting = null;
            if (string.IsNullOrEmpty(channelKey))
                setting = _setting; // root level
            else // channel level
            {
                if (_setting.Channels.TryGetValue(channelKey, out ActivationUnit unit))
                    setting = unit;
            }
            return setting != null;
        }

        #endregion // TryGetSettingUnit

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
