using System;
using System.Xml.Serialization;

namespace FactoriesManager.NUnit
{
    [Serializable]
    [XmlType(TypeName = "reasonType")]
    public class ReasonType
    {
        [XmlElement("message")]
        public string Message { get; set; }
    }
}