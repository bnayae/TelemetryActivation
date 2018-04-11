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
using Contracts;

namespace TelemetryTests
{
    [TestClass]
    public class ApiDesignTestsB
    {
        private IMetricsReporterFactory _reporterFactory = A.Fake<IMetricsReporterFactory>();
        private IMetricsReporterAdvanceFactory _reporterAdvanceFactory = A.Fake<IMetricsReporterAdvanceFactory>();

        [TestMethod]
        public void IMetricsReporter_Test()
        {
            //IMetricsReporter _reporter = 
            //    _reporterFactory.ForContext<ApiDesignTestsB>();
            IMetricsReporter _reporter = 
                _reporterFactory.ForContext("Mongo");
            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
            {
                ["Active"] = "true",
                ["Sentiment"] = "happy"
            };

            _reporter.Count();
            _reporter.Count(ImportanceLevel.High);
            _reporter.Count(ImportanceLevel.High, tags);
            _reporter.Count(tags: tags, operationName: "X");

            using (_reporter.Duration())
            {
            }
        }

        [TestMethod]
        public void IMetricsReporterAdvance_Test()
        {
            // IMetricsReporterAdvance _reporter = 
            //    _reporterAdvanceFactory.ForContext<ApiDesignTestsB>();
            IMetricsReporterAdvance _reporter =
                _reporterAdvanceFactory.ForContext("Mongo");

            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
            {
                ["Active"] = "true",
                ["Sentiment"] = "happy"
            };
            IReadOnlyDictionary<string, object> fields = new Dictionary<string, object>
            {
                ["Level"] = 12.54,
                ["Precision"] = 0.6
            };

            _reporter.Count();
            _reporter.Count(ImportanceLevel.High);
            _reporter.Count(ImportanceLevel.High, tags);
            _reporter.Count(ImportanceLevel.High, tags, 10);
            _reporter.Count(ImportanceLevel.High, tags, 10, "Web-Api");
            _reporter.Count(tags: tags, operationName: "Web-Api");
            _reporter.Report(fields: fields, tags: tags);
            _reporter.Report(fields, tags);
            _reporter.Report(fields, tags, ImportanceLevel.High);
        }

        [TestMethod]
        public void IMetricsReporterAdvance_Measurement_Test()
        {
            IMetricsReporterAdvance _reporter =
                _reporterAdvanceFactory.Measurement("Special")
                                        .ForContext("Mongo");

            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
            {
                ["Active"] = "true",
                ["Sentiment"] = "happy"
            };
            IReadOnlyDictionary<string, object> fields = new Dictionary<string, object>
            {
                ["Level"] = 12.54,
                ["Precision"] = 0.6
            };

            _reporter.Count();
            _reporter.Count(ImportanceLevel.High);
            _reporter.Count(ImportanceLevel.High, tags);
            _reporter.Count(ImportanceLevel.High, tags, 10);
            _reporter.Count(ImportanceLevel.High, tags, 10, "Web-Api");
            _reporter.Count(tags: tags, operationName: "Web-Api");
            _reporter.Report(fields: fields, tags: tags);
            _reporter.Report(fields, tags);
            _reporter.Report(fields, tags, ImportanceLevel.High);
        }
    }
}
