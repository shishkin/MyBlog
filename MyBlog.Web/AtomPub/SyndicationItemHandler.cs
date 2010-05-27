using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;

namespace MyBlog.Web.AtomPub
{
    public class SyndicationItemHandler : ISyndicationItemHandler
    {
        public bool CanCreateItem(IncomingWebRequestContext request)
        {
            return request.ContentType.StartsWith(ContentTypes.AtomEntry);
        }

        public SyndicationItem CreateItem(IncomingWebRequestContext request, Stream stream)
        {
            var item = SyndicationItem.Load(XmlReader.Create(stream));
            item.Id = HttpUtility.UrlEncode(item.Title.Text);
            return item;
        }
    }
}