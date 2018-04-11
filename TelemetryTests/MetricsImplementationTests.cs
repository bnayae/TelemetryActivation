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

        [TestMethod]
        public void Implement_Via_Config_Test()
        {
            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
            {
                ["operationName"] = "X",
                ["operationGroup"] = "Y"
            };

            var activationFactory = new ConfigActivationProvider();
            var simpleConfig = new SimpleConfig();
            IMetricsReporterBuilder factory = new MetricsReporterBuilder(
                activationFactory, 
                simpleConfig);

            // TODO: don't use cast
            var activation = (TelemetryActivation)activationFactory.Create();
            activation.TryAppendToken("controller:ping");
            activation.TryAppendToken("controller-action:ping");
            using (var reporter =
                factory.ForContext<MetricsImplementationTests>())
            {
                reporter.Count(ImportanceLevel.High, tags);
            }
        }
    }
}
