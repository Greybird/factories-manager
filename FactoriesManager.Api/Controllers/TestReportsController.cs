using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using FactoriesManager.Api.Binders;
using FactoriesManager.Api.Exceptions;
using FactoriesManager.Api.Results;
using FactoriesManager.Extensions;
using FactoriesManager.Model;
using Hangfire;
using Swashbuckle.Swagger.Annotations;
using TestReport = FactoriesManager.Api.Models.TestReport;

namespace FactoriesManager.Api.Controllers
{
    /// <summary>
    /// Expose test reports manipulation operations.
    /// A test report is a logical unit of tests statuses, which can be aggregated from a single source in several parts.
    /// </summary>
    [RoutePrefix("api/testreports")]
    public class TestReportsController : ApiController
    {
        private const string CreateReportRouteName = "CreateReport";
        private const string SelectReportRouteName = "SelectReport";
        private const string CommitReportRouteName = "CommitReport";
        private const string CreateTestResultRouteName = "CreateTestResult";
        private const string SelectTestResultRouteName = "SelectTestResult";
        private const string UpdateTestResultRouteName = "UpdateTestResult";
        private const string DeleteTestResultRouteName = "DeleteTestResult";

        private const string SelectJobStatusRouteName = "SelectJobStatus";

        private const string TestReportsSwaggerTag = "Test Reports";
        private const string TestResultsSwaggerTag = "Test Results";
        private const string JobsSwaggerTag = "Jobs";

        /// <summary>
        /// Creates a test report
        /// </summary>
        /// <remarks>
        /// A test report is a numbered collection of test results from a source.<br/>
        /// It is identified through an id which is expected to be incremented for each report (not necessarilly by one).<br/>
        /// Once created, test results can be added to the report, and then commited.
        /// </remarks>
        /// <param name="source">the source name</param>
        /// <param name="reportId">an incremental id to identify the report for the source</param>
        /// <response code="201">Test Report created</response>
        /// <response code="422">The id used is already in used, or is lower than the one last processed</response>
        [HttpPut]
        [Route("{source}/{reportId}", Name = CreateReportRouteName)]
        [ResponseType(typeof(TestReport))]
        [SwaggerOperation(Tags = new[] {TestReportsSwaggerTag})]
        public async Task<CreatedAtRouteNegotiatedContentResult<TestReport>> CreateReport([FromUri] [Required] string source,
            [FromUri] [Required] int reportId)
        {
            var container = new TestsModelContainer();
            var report = await container.CreateTestReport(source, reportId);
            return CreatedAtRoute(
                CreateReportRouteName,
                new Dictionary<string, object>
                {
                    {"source", source},
                    {"reportId", reportId}
                },
                FromTestReport(report));
        }

        /// <summary>
        /// Gets a test report
        /// </summary>
        /// <param name="source">the source name</param>
        /// <param name="reportId">an incremental id to identify the report for the source</param>
        /// <response code="200">Test Report</response>
        /// <response code="404">Test report not found</response>
        [HttpGet]
        [Route("{source}/{reportId}", Name = SelectReportRouteName)]
        [ResponseType(typeof(TestReport))]
        [SwaggerOperation(Tags = new[] {TestReportsSwaggerTag})]
        public async Task<OkNegotiatedContentResult<TestReport>> GetReport([FromUri] [Required] string source, [FromUri] [Required] int reportId)
        {
            var container = new TestsModelContainer();
            var report = await container.GetTestReport(source, reportId);
            return Ok(FromTestReport(report));
        }

        /// <summary>
        /// Retrieves a test result data
        /// </summary>
        /// <remarks>
        /// A test result is the bare data that will be parsed to produce actual test statuses.
        /// </remarks>
        /// <param name="source">the source name</param>
        /// <param name="reportId">the report id</param>
        /// <param name="resultId">the result id</param>
        /// <response code="200">The content</response>
        /// <response code="404">Content not found</response>
        [HttpGet]
        [Route("{source}/{reportId}/results/{resultId}", Name = SelectTestResultRouteName)]
        [ResponseType(typeof(string))]
        [SwaggerOperation(Tags = new[] {TestResultsSwaggerTag})]
        public async Task<OkNegotiatedContentResult<string>> GetTestResult([FromUri] [Required] string source, [FromUri] [Required] int reportId,
            [FromUri] [Required] int resultId)
        {
            var container = new TestsModelContainer();
            var result = await container.GetTestResult(source, reportId, resultId);
            return Ok(result.Content);
        }

        /// <summary>
        /// Adds a test result data
        /// </summary>
        /// <remarks>
        /// A test result is the bare data that will be parsed to produce actual test statuses.
        /// </remarks>
        /// <param name="source">the source name</param>
        /// <param name="reportId">the report id</param>
        /// <param name="testResults">the test results</param>
        /// <response code="201">Results created</response>
        /// <response code="404">Report not found</response>
        /// <response code="422">Report already closed</response>
        [HttpPost]
        [Route("{source}/{reportId}/results", Name = CreateTestResultRouteName)]
        [ResponseType(typeof(string))]
        [SwaggerOperation(Tags = new[] {TestResultsSwaggerTag})]
        public async Task<CreatedAtRouteNegotiatedContentResult<string>> CreateTestResult([FromUri] [Required] string source, [FromUri] [Required] int reportId,
            [FromRawBody] [Required(AllowEmptyStrings = false)] string testResults)
        {
            var container = new TestsModelContainer();
            var results = await container.CreateTestResult(source, reportId, testResults);
            return CreatedAtRoute(
                SelectTestResultRouteName,
                new Dictionary<string, object>
                {
                    {"source", source},
                    {"reportId", reportId},
                    {"resultId", results.Id}
                }, results.Content);
        }

        /// <summary>
        /// Updates a test result data
        /// </summary>
        /// <param name="source">the source name</param>
        /// <param name="reportId">the report id</param>
        /// <param name="resultId">the result id</param>
        /// <param name="testResults">the test results</param>
        /// <response code="200">Results updated</response>
        /// <response code="404">Report not found or results not found</response>
        /// <response code="422">Report already closed</response>
        [HttpPut]
        [Route("{source}/{reportId}/results/{resultId}", Name = UpdateTestResultRouteName)]
        [ResponseType(typeof(string))]
        [SwaggerOperation(Tags = new[] {TestResultsSwaggerTag})]
        public async Task<OkNegotiatedContentResult<string>> UpdateTestResult([FromUri] [Required] string source, [FromUri] [Required] int reportId,
            [FromUri] [Required] int resultId, [FromRawBody] [Required(AllowEmptyStrings = false)] string testResults)
        {
            var container = new TestsModelContainer();
            var results = await container.UpdateTestResult(source, reportId, resultId, testResults);
            return Ok(results.Content);
        }

        /// <summary>
        /// Deletes a test result data
        /// </summary>
        /// <param name="source">the source name</param>
        /// <param name="reportId">the report id</param>
        /// <param name="resultId">the result id</param>
        /// <response code="204">Results deleted</response>
        /// <response code="404">Report not found or results not found</response>
        /// <response code="422">Report already closed</response>
        [HttpDelete]
        [Route("{source}/{reportId}/results/{resultId}", Name = DeleteTestResultRouteName)]
        [ResponseType(typeof(string))]
        [SwaggerOperation(Tags = new[] {TestResultsSwaggerTag})]
        public async Task DeleteTestResult([FromUri] [Required] string source, [FromUri] [Required] int reportId,
            [FromUri] [Required] int resultId)
        {
            var container = new TestsModelContainer();
            await container.DeleteTestResult(source, reportId, resultId);
        }

        /// <summary>
        /// Computes the test report
        /// </summary>
        /// <remarks>
        /// Results will be integrated into the global tests repository
        /// </remarks>
        /// <param name="source">the source name</param>
        /// <param name="reportId">the report id</param>
        /// <response code="202">Accepted</response>
        /// <response code="404">Report not found</response>
        /// <response code="422">Report already closed</response>
        [HttpPost]
        [Route("{source}/{reportId}/commit", Name = CommitReportRouteName)]
        [ResponseType(typeof(TestReport))]
        [SwaggerOperation(Tags = new[] {TestReportsSwaggerTag})]
        public async Task<AcceptedNegotiatedContentResult<TestReport>> CommitReport([FromUri] [Required] string source,
            [FromUri] [Required] int reportId)
        {
            var jobId = BackgroundJob.Enqueue(() => CommitReport(source, reportId, JobCancellationToken.Null));
            return new AcceptedNegotiatedContentResult<TestReport>(
                this,
                SelectJobStatusRouteName,
                new Dictionary<string, object>
                {
                    {"jobId", jobId}
                }
            );
        }

        /// <summary>
        /// Gets a job status
        /// </summary>
        /// <param name="jobId">the job id</param>
        /// <response code="200">Job Id</response>
        /// <response code="404">Test report not found</response>
        [HttpGet]
        [Route("jobs/{jobId}", Name = SelectJobStatusRouteName)]
        [ResponseType(typeof(string))]
        [SwaggerOperation(Tags = new[] {JobsSwaggerTag})]
        public async Task<OkNegotiatedContentResult<string>> GetJob([FromUri] [Required] string jobId)
        {
            var jobData = JobStorage.Current.GetConnection().GetJobData(jobId);
            if (jobData == null)
            {
                throw new JobNotFoundException();
            }
            return Ok(jobData.State);
        }

        private TestReport FromTestReport(Model.TestReport info)
        {
            return new TestReport
            {
                Source = info.Name,
                Id = info.Id,
                Status = info.Status,
                Results = info.TestRunResults.Select(runResult => Url.Link(SelectTestResultRouteName, new Dictionary<string, object>
                {
                    {"source", info.Name},
                    {"reportId", info.Id},
                    {"resultId", runResult.Id}
                })).ToList()
            };
        }

        public async Task CommitReport(string source, int reportId, IJobCancellationToken cancellationToken)
        {
            var container = new TestsModelContainer();
            var done = await container.Process(source, reportId, cancellationToken.ShutdownToken);
            if (!done)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}