using System;
using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telemetry.Providers.ConfigFile;
using Telemetry.Implementation;

namespace TelemetryTests
{
    [TestClass]
    public class MetricsImplementationTests
    {
        private readonly ITelemetryActivationContext _activationContext = TelemetryActivationContext.Default;
        private readonly ITelemetryTagContext _tagContext = TelemetryTagContext.Default;
        private readonly ISimpleConfig _simpleConfig = new SimpleConfig();
        private readonly ITelemetryPushContext _telemetryPushContext = 
                        new TelemetryPushContext(
                                            TelemetryActivationContext.Default, 
                                            TelemetryTagContext.Default);
        [TestMethod]
        public void Implement_Via_Config_Expand_Test()
        {
            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
            {
                ["operationName"] = "X",
                ["operationGroup"] = "Y"
            };

            var activationFactory = new ConfigActivationProvider(_activationContext);
            IMetricsReporterBuilder builder = new MetricsReporterBuilder(
                activationFactory, 
                _simpleConfig,
                _tagContext);

            _activationContext.PushFlow(CommonLayerOrService.WebApi, "Test-class", "test-method-a");
            _activationContext.PushToken("env", "qa");
            _tagContext.PushFlow(CommonLayerOrService.WebApi, "Test-class", "test-method-a");
            _tagContext.PushToken("env", "qa");
            _telemetryPushContext.PushToken("dry", "pushed-to-all");
            using (var reporter = builder.Build())
            {
                reporter.Count(ImportanceLevel.Low, tags);
            }
        }
        [TestMethod]
        public void Implement_Via_Config_Constrict_Test()
        {
            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
            {
                ["operationName"] = "X",
                ["operationGroup"] = "Y"
            };

            var activationFactory = new ConfigActivationProvider(_activationContext);
            IMetricsReporterBuilder builder = new MetricsReporterBuilder(
                activationFactory, 
                _simpleConfig,
                _tagContext);

            _activationContext.PushFlow(CommonLayerOrService.WebApi, "Test-class", "test-method");
            _activationContext.PushToken("env", "qa");
            _tagContext.PushFlow(CommonLayerOrService.WebApi, "Test-class", "test-method-b");
            _tagContext.PushToken("env", "qa");
            _telemetryPushContext.PushToken("dry", "pushed-to-all");
            using (var reporter = builder.Build())
            {
                reporter.Count(ImportanceLevel.High, tags);
            }
        }
    }
}
