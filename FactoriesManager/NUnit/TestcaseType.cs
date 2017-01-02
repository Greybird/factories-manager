using System;
using System.Xml.Serialization;

namespace FactoriesManager.NUnit
{
    [Serializable]
    [XmlType(TypeName = "test-caseType")]
    public class TestCaseType
    {
        [XmlArrayItem("category", IsNullable = false)]
        public CategoryType[] Categories { get; set; }

        [XmlArrayItem("property", IsNullable = false)]
        public PropertyType[] Properties { get; set; }

        [XmlElement("failure", typeof(FailureType))]
        public FailureType Failure { get; set; }

        [XmlElement("reason", typeof(ReasonType))]
        public ReasonType Reason { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("success")]
        public string Success { get; set; }

        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlAttribute("executed")]
        public string Executed { get; set; }

        [XmlAttribute("asserts")]
        public string Asserts { get; set; }

        [XmlAttribute("result")]
        public string Result { get; set; }
    }
}