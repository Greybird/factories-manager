using System;
using System.Xml.Serialization;

namespace FactoriesManager.NUnit
{
    [Serializable]
    [XmlType(TypeName = "categoryType")]
    public class CategoryType
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}