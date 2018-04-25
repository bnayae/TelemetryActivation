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

namespace Telemetry.Providers.ConfigFile
{
    /// <summary>
    /// Manage the context of the tags, 
    /// State build per execution flow (request).
    /// </summary>
    /// <seealso cref="Contracts.ITelemetryActivationContext" />
    public class TelemetryTagContext:
        ITelemetryTagContext
    {
        private readonly AsyncLocal<ImmutableDictionary<string, string>> _context =
            new AsyncLocal<ImmutableDictionary<string, string>>();

        #region Singleton

        public static readonly ITelemetryTagContext Default = new TelemetryTagContext();

        #endregion // Singleton

        #region Tags

        /// <summary>
        /// Tags .
        /// </summary>
        /// <example>
        /// env,qa
        /// flow,BL:LearningManager:GetLearningPath
        /// </example>
        public ImmutableDictionary<string, string> Tags => _context.Value ?? ImmutableDictionary<string, string>.Empty;

        #endregion // Tags

        #region PushToken

        /// <summary>
        /// Append general tokens.
        /// this information can be match by the activation rules.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="tokenValue">The token value.</param>
        /// <example>
        /// env,qa
        /// </example>
        public void PushToken<T>(string tokenKey, T tokenValue)
        {
            var tags = Tags;
            if (tags.ContainsKey(tokenKey))
                tags = tags.Remove(tokenKey);

            _context.Value = tags.Add(tokenKey, tokenValue?.ToString() ?? string.Empty);
        }

        #endregion // PushToken

        #region PushFlow

        #region Overloads

        /// <summary>
        /// Push execution flow information.
        /// this information can be match by the activation rules.
        /// </summary>
        /// <typeparam name="T">Use to represent name of the layer or service class</typeparam>
        /// <param name="layerOrService">The layer or service.</param>
        /// <param name="entryMethodName">Name of the entry method.</param>
        /// <param name="alternativeLayerOrService">When layerOrService == Other you can specify alternative layerOrService name.</param>
        /// <example>
        /// flow,BL:LearningManager:GetLearningPath
        /// </example>
        public void PushFlow<T>(
            CommonLayerOrService layerOrService,
            [CallerMemberName]string entryMethodName = null,
            string alternativeLayerOrService = null)
        {
            PushFlow(layerOrService, typeof(T).Name, entryMethodName, alternativeLayerOrService);
        }

        #endregion // Overloads

        /// <summary>
        /// Push execution flow information.
        /// this information can be match by the activation rules.
        /// </summary>
        /// <param name="layerOrService">The layer or service.</param>
        /// <param name="layerOrServiceClassName">
        /// Name of the layer or service class.
        /// You should use nameof(TheClassName).
        /// </param>
        /// <param name="entryMethodName">Name of the entry method.</param>
        /// <param name="alternativeLayerOrService">
        /// When layerOrService == Other you can specify alternative layerOrService name.
        /// </param>
        /// <example>
        /// flow:BL:LearningManager:GetLearningPath
        /// </example>
        public void PushFlow(
            CommonLayerOrService layerOrService,
            string layerOrServiceClassName,
            [CallerMemberName]string entryMethodName = null,
            string alternativeLayerOrService = null)
        {
            #region Validation
            if (layerOrService != CommonLayerOrService.Other &&
                !string.IsNullOrEmpty(alternativeLayerOrService))
            {
                throw new ArgumentException($"{nameof(alternativeLayerOrService)} is not valid when {nameof(layerOrService)} != {nameof(CommonLayerOrService.Other)}");
            }
            #endregion // Validation

            #region string category = ...
            string category;
            if (layerOrService == CommonLayerOrService.Other)
                category = alternativeLayerOrService;
            else
                category = layerOrService.ToString();
            #endregion // string category = ...

            string candidate = $"flow:{category}:{layerOrServiceClassName}:{entryMethodName}";
            var tags = Tags;
            if (tags.ContainsKey(candidate))
                return;
            _context.Value = tags.Add(candidate, string.Empty);
        }

        #endregion // PushFlow
    }
}
