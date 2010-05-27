using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.IO;

namespace MyBlog.Web.AtomPub
{
    public interface ISyndicationItemHandler
    {
        bool CanCreateItem(IncomingWebRequestContext request);

        SyndicationItem CreateItem(IncomingWebRequestContext request, Stream stream);
    }
}