using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ITelemetryPushContext
    {
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
        void PushToken<T>(string tokenKey, T tokenValue);

        #endregion // PushToken

        #region PushFlow

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
        void PushFlow(
            CommonLayerOrService layerOrService, 
            string layerOrServiceClassName,
            [CallerMemberName]string entryMethodName = null,
            string alternativeLayerOrService = null);

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
        void PushFlow<T>(
            CommonLayerOrService layerOrService, 
            [CallerMemberName]string entryMethodName = null,
            string alternativeLayerOrService = null);

        #endregion // PushFlow
    }
}
