using System;
using System.Xml.Serialization;

namespace FactoriesManager.NUnit
{
    [Serializable]
    [XmlType(TypeName = "environmentType")]
    public class EnvironmentType
    {
        [XmlAttribute("nunit-version")]
        public string NUnitVersion { get; set; }

        [XmlAttribute("clr-version")]
        public string ClrVersion { get; set; }

        [XmlAttribute("os-version")]
        public string OsVersion { get; set; }

        [XmlAttribute("platform")]
        public string Platform { get; set; }

        [XmlAttribute("cwd")]
        public string Cwd { get; set; }

        [XmlAttribute("machine-name")]
        public string MachineName { get; set; }

        [XmlAttribute("user")]
        public string User { get; set; }

        [XmlAttribute("user-domain")]
        public string UserDomain { get; set; }
    }
}