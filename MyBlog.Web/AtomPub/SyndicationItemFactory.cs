using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace MyBlog.Web.AtomPub
{
    public class SyndicationItemFactory : ISyndicationItemFactory
    {
        private readonly ISyndicationItemHandler[] handlers = new ISyndicationItemHandler[]
        {
            new SyndicationItemHandler(),
            new MediaSyndicationItemHandler()
        };

        public SyndicationItem CreateItem(IncomingWebRequestContext request, Stream stream)
        {
            return handlers
                .Where(x => x.CanCreateItem(request))
                .FirstOrDefault()
                .CreateItem(request, stream);
        }
    }
}