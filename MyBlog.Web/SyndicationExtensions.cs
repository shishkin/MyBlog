using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using System.Text;

namespace MyBlog.Web
{
    public static class SyndicationExtensions
    {
        public static string GetText(this SyndicationContent content)
        {
            var textContent = content as TextSyndicationContent;
            if (textContent != null)
            {
                return textContent.Text;
            }
            var xmlContent = content as XmlSyndicationContent;
            if (xmlContent != null)
            {
                return xmlContent.GetReaderAtContent().ReadOuterXml();
            }
            return null;
        }

        public static SyndicationItem LoadSyndicationItem(this string xml)
        {
            using (var text = new StringReader(xml))
            using (var reader = XmlReader.Create(text))
            {
                return SyndicationItem.Load(reader);
            }
        }

        public static string SaveAsString(this SyndicationItem item)
        {
            using (var stream = new MemoryStream())
            {
                var writer = XmlWriter.Create(stream);
                item.SaveAsAtom10(writer);
                writer.Flush();
                stream.Position = 0;
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}