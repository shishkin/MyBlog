using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    public interface IResourceCollection
    {
        ResourceCollectionInfo Info { get; }

        SyndicationItem Get(string id);
        
        IEnumerable<SyndicationItem> List();

        SyndicationItem Post(SyndicationItem item);

        SyndicationItem Put(SyndicationItem item);

        void Delete(string id);
    }
}
