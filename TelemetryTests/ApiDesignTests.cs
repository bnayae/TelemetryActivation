//using System;
//using Contracts;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using FakeItEasy;
//using System.Runtime.CompilerServices;
//using System.Diagnostics;
//using System.IO;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TelemetryTests
//{
//    [TestClass]
//    public class ApiDesignTests
//    {
//        private IMetricsReporter _reporter = A.Fake<IMetricsReporter>();
//        private IMetricsReporterAdvance _advanceReporter = A.Fake<IMetricsReporterAdvance>();

//        [TestMethod]
//        public void IMetricsReporter_Test()
//        {
//            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
//            {
//                ["Active"] = "true",
//                ["Sentiment"] = "happy"
//            };

//            _reporter.Count();
//            _reporter.Count("Mongo");
//            _reporter.Count("Mongo", ImportanceLevel.High);
//            _reporter.Count("Mongo", ImportanceLevel.High, tags);
//            _reporter.Count("Mongo", tags: tags, operationGroup: "Web-Api");

//            using (_reporter.Duration())
//            {
//            }
//        }

//        [TestMethod]
//        public void IMetricsReporterAdvance_Test()
//        {
//            IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
//            {
//                ["Active"] = "true",
//                ["Sentiment"] = "happy"
//            };
//            IReadOnlyDictionary<string, object> fields = new Dictionary<string, object>
//            {
//                ["Level"] = 12.54,
//                ["Precision"] = 0.6
//            };

//            _advanceReporter.Count();
//            _advanceReporter.Count("Mongo");
//            _advanceReporter.Count("Mongo", ImportanceLevel.High);
//            _advanceReporter.Count("Mongo", ImportanceLevel.High, tags);
//            _advanceReporter.Count("Mongo", ImportanceLevel.High, tags, 10);
//            _advanceReporter.Count("Mongo", ImportanceLevel.High, tags, 10, "Web-Api");
//            _advanceReporter.Count("Mongo", tags: tags, operationGroup: "Web-Api");

//            _advanceReporter.Report(fields: fields, tags: tags);
//            _advanceReporter.Report("Mongo", fields, tags);
//            _advanceReporter.Report("Mongo", fields, tags, ImportanceLevel.High, "Web-Api");
//        }
//    }
//}
