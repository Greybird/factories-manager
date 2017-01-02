using System;
using System.Linq;
using FactoriesManager.Model;
using Swashbuckle.Swagger;
using TestReport = FactoriesManager.Api.Models.TestReport;

namespace FactoriesManager.Api.Filters.Doc
{
    internal class TestReportDefaultsFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            schema.@default = new TestReport
            {
                Source = "TestSource",
                Id = 12,
                Status = ReportStatus.Created,
                Results = Enumerable.Range(1, 3).Select(i => "http://some.url/" + i).ToList()
            };
        }
    }
}