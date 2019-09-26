using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WikiDumpParser.Models;

namespace WikiDumpParser
{
    /// <summary>
    /// A parser of the XML Wiki dump
    /// </summary>
    public class Parser
    {
        readonly XmlReader xmlReader;

        public Siteinfo SiteInfo { get; private set; }

        private Parser(Stream stream)
        {
            xmlReader = XmlReader.Create(stream);
        }

        private void ReadSiteInfo()
        {
            xmlReader.ReadToFollowing("siteinfo");

            XmlSerializer serializer = new XmlSerializer(typeof(Siteinfo));
            SiteInfo = (Siteinfo)serializer.Deserialize(xmlReader);
        }

        /// <summary>
        /// Enumerate the articles in the database dump
        /// </summary>
        public IEnumerable<Page> ReadPages()
        {
            XNamespace nss = "http://www.mediawiki.org/xml/export-0.10/";
            
            while (xmlReader.ReadToFollowing("page"))
            {
                XElement el = XNode.ReadFrom(xmlReader) as XElement;

                XElement titleNode = el.Element(nss + "title");
                XElement nsNode = el.Element(nss + "ns");
                XElement idNode = el.Element(nss + "id");
                XElement redirectNode = el.Element(nss + "redirect");
                XElement revisionNode = el.Element(nss + "revision");
                XElement textNode = revisionNode.Element(nss + "text");

                string title = titleNode.Value;
                int nsKey = int.Parse(nsNode.Value);
                int id = int.Parse(idNode.Value);
                string redirectTitle = redirectNode?.Attribute("title").Value;
                string text = textNode.Value;

                Page page = new Page(id, nsKey, redirectTitle, title, text);

                yield return page;
            }
        }

        public static Parser Create(Stream stream)
        {
            Parser parser = new Parser(stream);
            parser.ReadSiteInfo();

            return parser;
        }
    }
}
