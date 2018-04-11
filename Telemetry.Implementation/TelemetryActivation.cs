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

// TODO: ToLowwer()

namespace Telemetry.Providers.ConfigFile
{
    [DebuggerTypeProxy(typeof(DebugView))]
    public class TelemetryActivation :
        ITelemetryActivation,
        ITelemetryActivationContext
    {
        private readonly AsyncLocal<ImmutableHashSet<string>> _context =
                                        new AsyncLocal<ImmutableHashSet<string>>();

        private readonly ActivationSetting _setting;

        #region Ctor

        public TelemetryActivation(ActivationSetting setting)
        {
            _setting = setting;
        }

        #endregion // Ctor

        #region Tokens

        public ImmutableHashSet<string> Tokens => _context.Value;

        #endregion // Tokens

        #region TryAppendToken

        public bool TryAppendToken<T>(T token)
        {
            string candidate = token?.ToString();
            var tokens = Tokens;
            if (tokens.Contains(candidate))
                return false;

            _context.Value = tokens.Add(candidate);
            return true;
        }

        #endregion // TryAppendToken

        #region HasToken

        public bool HasToken(string token) => Tokens.Contains(token);

        #endregion // HasToken

        #region IsActive

        public bool IsActive(
            ImportanceLevel metricLevel,
            string ComponentTag = null)
        {
            if (metricLevel < _setting.MinImportance)
            {
                #region Check if pass the extends

                foreach (var extend in _setting.Extends)
                {
                    var settingImportance = extend.Importance;
                    if (settingImportance > metricLevel)
                        continue; // won't activate anyway

                    if (!string.IsNullOrEmpty(extend.ComponentTag) &&
                        extend.ComponentTag != ComponentTag)
                    {
                        continue;
                    }

                    var tokenSupportAllFilters =
                        extend.Filters.All(m => HasToken(m.Path));
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

                if (!string.IsNullOrEmpty(constrict.ComponentTag) &&
                    constrict.ComponentTag != ComponentTag)
                {
                    continue;
                }

                var tokenSupportAllFilters =
                    constrict.Filters.All(m => HasToken(m.Path));
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
