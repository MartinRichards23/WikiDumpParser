using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace WikiDumpParser.Models
{
    [XmlRoot(ElementName = "namespace", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
    public class Namespace
    {
        [XmlAttribute(AttributeName = "key")]
        public int Key { get; set; }

        [XmlAttribute(AttributeName = "case")]
        public string Case { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "namespaces", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
    public class Namespaces
    {
        [XmlElement(ElementName = "namespace", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public List<Namespace> Namespace { get; set; }
    }

    [XmlRoot(ElementName = "siteinfo", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
    public class Siteinfo
    {
        [XmlElement(ElementName = "sitename", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public string Sitename { get; set; }

        [XmlElement(ElementName = "dbname", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public string Dbname { get; set; }

        [XmlElement(ElementName = "base", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public string Base { get; set; }

        [XmlElement(ElementName = "generator", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public string Generator { get; set; }

        [XmlElement(ElementName = "case", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public string Case { get; set; }

        [XmlElement(ElementName = "namespaces", Namespace = "http://www.mediawiki.org/xml/export-0.10/")]
        public Namespaces Namespaces { get; set; }
    }
}
