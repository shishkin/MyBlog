using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    using Models;

    public class ResourceCollection
        : IResourceCollection
    {
        private readonly IDataStore store;

        public ResourceCollection(IDataStore store, ResourceCollectionInfo info)
        {
            this.store = store;
            Info = info;
        }

        public ResourceCollectionInfo Info { get; private set; }

        public SyndicationItem Post(SyndicationItem item)
        {
            store.Put(item);
            return item;
        }

        public SyndicationItem Put(SyndicationItem item)
        {
            return Post(item);
        }

        public void Delete(string id)
        {
            store.Delete(id);
        }

        public IEnumerable<SyndicationItem> List()
        {
            return store.List();
        }

        public SyndicationItem Get(string id)
        {
            return store.Get(id);
        }
    }
}
