using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    public interface ISyndicationItemFactory
    {
        SyndicationItem CreateItem(IncomingWebRequestContext request, Stream stream);
    }
}
