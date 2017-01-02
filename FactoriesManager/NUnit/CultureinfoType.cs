using System;
using System.Xml.Serialization;

namespace FactoriesManager.NUnit
{
    [Serializable]
    [XmlType(TypeName = "culture-infoType")]
    public class CultureInfoType
    {
        [XmlAttribute("current-culture")]
        public string Currentculture { get; set; }

        [XmlAttribute("current-uiculture")]
        public string Currentuiculture { get; set; }
    }
}