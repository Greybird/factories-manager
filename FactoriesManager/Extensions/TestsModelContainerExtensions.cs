using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using FactoriesManager.Exceptions;
using FactoriesManager.Model;

namespace FactoriesManager.Extensions
{
    public static partial class TestsModelContainerExtensions
    {
        public static Task<TestReport> CreateTestReport(this TestsModelContainer container, string name, int reportId)
        {
            return RunWithConcurrencyRetry(async () =>
                {
                    var testSource = await GetOrCreateSource(container, name);
                    if (reportId < testSource.LastReportId)
                    {
                        throw new OutdatedReportIdException();
                    }
                    var report = await GetTestReportInternal(container, name, reportId, Operation.Read, false);
                    if (report != null)
                    {
                        throw new OutdatedReportIdException();
                    }
                    var testReport = container.TestReports.Create();
                    testReport.Name = name;
                    testReport.Id = reportId;
                    testReport.TestSource = testSource;
                    testSource.LastReportId = reportId;
                    container.TestReports.Add(testReport);
                    container.SaveChanges();
                    return testReport;
                }
            );
        }

        public static async Task<TestReport> GetTestReport(this TestsModelContainer container, string name, int reportId)
        {
            return await GetTestReportInternal(container, name, reportId, Operation.Read);
        }

        private static async Task<TestReport> GetTestReportInternal(TestsModelContainer container, string name, int reportId, Operation operation,
            bool throwIfNotExist = true)
        {
            var report = await container.TestReports.FindAsync(reportId, name);
            if (report == null)
            {
                if (throwIfNotExist)
                {
                    throw new ReportNotFoundException();
                }
            }
            else
            {
                switch (operation)
                {
                    case Operation.Read:
                        break;
                    case Operation.Write:
                        switch (report.Status)
                        {
                            case ReportStatus.Created:
                                break;
                            case ReportStatus.Processing:
                            case ReportStatus.Closed:
                            case ReportStatus.Error:
                                throw new ClosedReportException();
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case Operation.Commit:
                        switch (report.Status)
                        {
                            case ReportStatus.Created:
                            case ReportStatus.Processing:
                                break;
                            case ReportStatus.Closed:
                            case ReportStatus.Error:
                                throw new ClosedReportException();
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
                }
            }
            return report;
        }

        public static Task<TestRunResult> CreateTestResult(this TestsModelContainer container, string name, int reportId, string content)
        {
            return RunWithConcurrencyRetry(async () =>
                {
                    var testReport = await GetTestReportInternal(container, name, reportId, Operation.Write);
                    if (testReport == null)
                    {
                        throw new ReportNotFoundException();
                    }
                    var result = container.TestRunResults.Create();
                    result.SourceName = name;
                    result.ReportId = reportId;
                    result.Id = ++testReport.LastRunResultId;
                    result.Content = content;
                    container.TestRunResults.Add(result);
                    container.SaveChanges();
                    return result;
                }
            );
        }

        public static async Task<TestRunResult> GetTestResult(this TestsModelContainer container, string name, int reportId, int resultId)
        {
            var testReport = await GetTestReportInternal(container, name, reportId, Operation.Read);
            var result = await container.TestRunResults.FindAsync(resultId, testReport.Id, testReport.Name);
            if (result == null)
            {
                throw new ResultsNotFoundException();
            }
            return result;
        }

        private static async Task<TestRunResult> GetTestResultInternal(this TestsModelContainer container, string name, int reportId, int resultId,
            Operation operation)
        {
            var testReport = await GetTestReportInternal(container, name, reportId, operation);
            var result = await container.TestRunResults.FindAsync(resultId, testReport.Id, testReport.Name);
            if (result == null)
            {
                throw new ResultsNotFoundException();
            }
            return result;
        }

        public static Task<TestRunResult> UpdateTestResult(this TestsModelContainer container, string name, int reportId, int resultId, string content)
        {
            return RunWithConcurrencyRetry(async () =>
            {
                var result = await GetTestResultInternal(container, name, reportId, resultId, Operation.Write);
                result.Content = content;
                container.SaveChanges();
                return result;
            });
        }

        public static async Task DeleteTestResult(this TestsModelContainer container, string name, int reportId, int resultId)
        {
            var result = await GetTestResultInternal(container, name, reportId, resultId, Operation.Write);
            container.TestRunResults.Remove(result);
            container.SaveChanges();
        }

        private static async Task<TestSource> GetOrCreateSource(TestsModelContainer container, string name)
        {
            return await RunWithConcurrencyRetry(async () =>
                {
                    var testSource = container.TestSources.Find(name);
                    if (testSource == null)
                    {
                        testSource = container.TestSources.Create();
                        testSource.Name = name;
                        container.TestSources.Add(testSource);
                        await container.SaveChangesAsync();
                    }
                    return testSource;
                }
            );
        }

        private enum Operation
        {
            Read,
            Write,
            Commit
        }

        private static Task<T> RunWithConcurrencyRetry<T>(Func<Task<T>> getObject)
        {
            while (true)
            {
                try
                {
                    return getObject();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Task.WaitAll(ex.Entries.Select(e => e.ReloadAsync()).ToArray());
                }
            }
        }
    }
}