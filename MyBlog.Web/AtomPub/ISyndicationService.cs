using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    public interface ISyndicationService
    {
        ServiceDocument ServiceDocument();

        SyndicationFeed Feed(string collectionName);

        SyndicationItem Get(string collectionName, string id);

        SyndicationItem Post(string collectionName, SyndicationItem item);

        SyndicationItem Put(string collectionName, SyndicationItem item);

        void Delete(string collectionName, string id);
    }
}
