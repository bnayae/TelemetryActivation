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
    /// Manage the context of the activation state, 
    /// State build per execution flow (request).
    /// </summary>
    /// <seealso cref="Contracts.ITelemetryActivationContext" />
    public class TelemetryActivationContext:
        ITelemetryActivationContext
    {
        private readonly AsyncLocal<ImmutableHashSet<string>> _context =
                                        new AsyncLocal<ImmutableHashSet<string>>();

        #region Singleton

        public static readonly ITelemetryActivationContext Default = new TelemetryActivationContext(); 

        #endregion // Singleton
       
        #region Tokens

        /// <summary>
        /// Tokens trail path of the execution 
        /// and environment metadata.
        /// </summary>
        /// <example>
        /// env:qa
        /// controller:adaptive-learning
        /// controller-action:get-learningPath
        /// bl:learning
        /// bl-action:get-learningThePath
        /// sp:sp_select_learning-path
        /// </example>
        public ImmutableHashSet<string> Tokens => _context.Value ?? ImmutableHashSet<string>.Empty;

        #endregion // Tokens

        #region PushToken

        /// <summary>
        /// Append general tokens.
        /// this information can be match by the activation rules.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="tokenValue">The token value.</param>
        /// <example>
        /// env:qa
        /// </example>
        public void PushToken<T>(string tokenKey, T tokenValue)
        {
            string candidate = $"{tokenKey}:{tokenValue?.ToString()}";
            var tokens = Tokens;
            if (HasToken(candidate))
                return;

            _context.Value = tokens.Add(candidate);
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
        /// flow:BL:LearningManager:GetLearningPath
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

            string candidateRoot = $"flow:{category}:{layerOrServiceClassName}";
            string candidate = $"{candidateRoot}:{entryMethodName}";
            var tokens = Tokens;
            if (!tokens.Contains(candidateRoot))
                _context.Value = tokens.Add(candidateRoot);
            if (!tokens.Contains(candidate))
                _context.Value = tokens.Add(candidate);
        }

        #endregion // PushFlow

        #region HasToken

        /// <summary>
        /// Determines whether the specified token exists.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <c>true</c> if has token; otherwise, <c>false</c>.
        /// </returns>
        public bool HasToken(string token) => Tokens.Contains(token, StringComparer.OrdinalIgnoreCase);

        #endregion // HasToken
    }
}
