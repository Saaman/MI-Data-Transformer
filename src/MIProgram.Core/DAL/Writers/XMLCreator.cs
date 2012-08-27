using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MIProgram.Core.Model;

namespace MIProgram.Core.DAL.Writers
{
    public class XMLCreator
    {
        private readonly IWriter _writer;

        private const string ReviewLink = "http://www.metal-impact.com/index.php?name=Reviews&req=showcontent&id={0}";
        private const string XMLLink = "http://www.metal-impact.com/{0}/{1}.xml";
        private readonly XNamespace _siteMapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

        private readonly string _destRootDir;

        public XMLCreator(IWriter writer, string destRootDir)
        {
            _writer = writer;
            _destRootDir = destRootDir;
        }

        public XDocument CreateSingleXML(Album album)
        {
            var xml = new XElement("review",
                new XElement("album_name", new XCData(album.Title)),
                new XElement("artist_name", new XCData(album.Artist.Name)),
                new XElement("deezer_album_name", new XCData(album.DeezerAlbum)),
                new XElement("deezer_artist_name", new XCData(album.DeezerArtist)),
                new XElement("review", new XCData(album.ReviewText)),
                new XElement("id_mireview", new XCData(album.Id.ToString())),
                new XElement("reviewer", new XCData(album.Reviewer.Name)),
                new XElement("mireviewlink", new XCData(string.Format(ReviewLink, album.Id)))
                );

            return new XDocument(new XDeclaration("1.0", "UTF-_8", "yes"), xml);
        }

        public XDocument CreateReport(IList<XElement> elements)
        {
            return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("metal_impact", elements));
        }

        public XDocument CreateSiteMap(IList<XElement> elements)
        {
            return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement(_siteMapNs + "urlset", elements));
        }

        public XElement GetXmlForReport(int id)
        {
            return new XElement("review", new XElement("id_mireview", new XCData(id.ToString())),
                new XElement("xml_review", new XCData(string.Format(XMLLink, _destRootDir, id))));
        }

        public void Publish(XDocument xDoc, string fileNameWithoutExtension)
        {
            Publish(xDoc, fileNameWithoutExtension, string.Empty);
        }

        public void Publish(XDocument xDoc, string fileNameWithoutExtension, string rootDir)
        {
            _writer.WriteXML(xDoc, fileNameWithoutExtension, rootDir);
        }

        public XElement GetXmlForSitemap(DateTime lastUpdateDate, int id)
        {
            string url = string.Format(ReviewLink, id);
            return new XElement(_siteMapNs + "url", new XElement(_siteMapNs + "loc", url),
                new XElement(_siteMapNs + "lastmod", string.Format("{0:yyyy-MM-dd}", lastUpdateDate)),
                new XElement(_siteMapNs + "changefreq", string.Empty),
                new XElement(_siteMapNs + "priority", string.Empty));
        }
    }
}
