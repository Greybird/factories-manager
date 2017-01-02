using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FactoriesManager.Model;
using FactoriesManager.NUnit;

namespace FactoriesManager.Extensions
{
    public static partial class TestsModelContainerExtensions
    {
        public static async Task<bool> Process(this TestsModelContainer container, string name, int reportId, CancellationToken cancellationToken)
        {
            return await RunWithConcurrencyRetry(() => ProcessInternal(container, name, reportId, cancellationToken));
        }

        private static async Task<bool> ProcessInternal(this TestsModelContainer container, string name, int reportId, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            var report = await GetTestReportInternal(container, name, reportId, Operation.Commit);

            UpdateReportStatusIfNeeded(container, report);

            var parsedTests = ParseTests(container, report, cancellationToken);
            if (parsedTests.Cancelled)
            {
                return false;
            }
            if (parsedTests.Exception != null)
            {
                report.Status = ReportStatus.Error;
            }
            else
            {
                SaveTests(container, name, parsedTests);
            }
            container.SaveChanges();
            return true;
        }

        private static void UpdateReportStatusIfNeeded(TestsModelContainer container, TestReport report)
        {
            if (report.Status < ReportStatus.Processing)
            {
                report.Status = ReportStatus.Processing;
                container.SaveChanges();
            }
        }

        private static TestsParsingResult ParseTests(TestsModelContainer container, TestReport report, CancellationToken cancellationToken)
        {
            var results = new TestsParsingResult();
            try
            {
                foreach (var testRunResult in report.TestRunResults)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        results.Cancelled = true;
                        break;
                    }
                    results.Tests.AddRange(GetTests(container, testRunResult));
                }
            }
            catch (Exception e)
            {
                results.Exception = e;
            }
            return results;
        }

        private static IEnumerable<Test> GetTests(TestsModelContainer container, TestRunResult testRunResult)
        {
            var xml = new XmlSerializer(typeof(TestResultsType));
            using (var s = new StringReader(testRunResult.Content))
            {
                var results = (TestResultsType) xml.Deserialize(s);
                yield break;
            }
        }

        private static void SaveTests(TestsModelContainer container, string name, TestsParsingResult parsedTests)
        {
            throw new NotImplementedException();
        }

        private class TestsParsingResult
        {
            public List<Test> Tests { get; }
            public bool Cancelled { get; set; }
            public Exception Exception { get; set; }

            public TestsParsingResult()
            {
                Tests = new List<Test>();
            }
        }
    }
}