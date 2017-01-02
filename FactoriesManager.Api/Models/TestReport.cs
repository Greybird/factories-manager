using System.Collections.Generic;
using FactoriesManager.Api.Filters.Doc;
using FactoriesManager.Model;
using Swashbuckle.Swagger.Annotations;

namespace FactoriesManager.Api.Models
{
    /// <summary>
    /// A Test report
    /// </summary>
    [SwaggerSchemaFilter(typeof(TestReportDefaultsFilter))]
    public class TestReport
    {
        public string Source { get; set; }
        public int Id { get; set; }
        public ReportStatus Status { get; set; }
        public List<string> Results { get; set; }
    }
}