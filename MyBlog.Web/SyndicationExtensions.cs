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
    }
}