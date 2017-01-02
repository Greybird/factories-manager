using System;
using System.Xml.Serialization;

namespace FactoriesManager.NUnit
{
    [Serializable]
    [XmlType(TypeName = "resultsType")]
    public class ResultsType
    {
        [XmlElement("test-case", typeof(TestCaseType))]
        public TestCaseType[] TestCases { get; set; }

        [XmlElement("test-suite", typeof(TestSuiteType))]
        public TestSuiteType[] TestSuites { get; set; }
    }
}