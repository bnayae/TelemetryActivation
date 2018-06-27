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

        public ImportanceLevel MetricThreshold => _setting.Threshold.Metric;

        public LogEventLevel TextualThreshold => _setting.Threshold.Textual;

        #region IsActive

        /// <summary>
        /// Determines whether the specified metric level is active.
        /// </summary>
        /// <param name="level">The metric level.</param>
        /// <returns>
        ///   <c>true</c> if the specified metric level is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsActive(
                ImportanceLevel level)
        {
            return IsActive((int)level, TelemetryActivationKind.Metric);

        }

        /// <summary>
        /// Determines whether the specified log level is active.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>
        /// <c>true</c> if the specified log level is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsActive(LogEventLevel level)
        {
            return IsActive((int)level, TelemetryActivationKind.Textual);
        }

        /// <summary>
        /// Determines whether the specified log level is active.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>
        /// <c>true</c> if the specified log level is active; otherwise, <c>false</c>.
        /// </returns>
        private bool IsActive(
                int level,
                TelemetryActivationKind kind)
        {
            #region int minLevel = ...

            int minLevel;
            switch (kind)
            {
                case TelemetryActivationKind.Metric:
                    minLevel = (int)_setting.Threshold.Metric;
                    break;
                case TelemetryActivationKind.Textual:
                    minLevel = (int)_setting.Threshold.Textual;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"invalid kind {kind}");
            }

            #endregion // int minLevel = ...

            ActivationUnit setting = _setting;
            if (level < minLevel)
            {
                // Disable unless extends (setting.TextualThreshold)
                #region Check if pass the extends

                foreach (var extend in setting.Extends)
                {
                    var extendLimit = extend.GetThreshold(kind);
                    var tokenSupportAllFilters =
                        extend.Filters.All(m => _activationContext.HasToken(m.Path));
                    if (tokenSupportAllFilters && extendLimit <= level)
                        return true;
                }
                return false;

                #endregion // If(Extend) true
            }

            // Enable unless constrict (setting.TextualThreshold)
            #region Check if constricted

            foreach (var constrict in setting.Constricts)
            {
                var constrictLimit = constrict.GetThreshold(kind);
                var tokenSupportAllFilters =
                    constrict.Filters.All(m => _activationContext.HasToken(m.Path));
                if (tokenSupportAllFilters && constrictLimit > level) 
                    return false; // disable when (both level and filters match)
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
