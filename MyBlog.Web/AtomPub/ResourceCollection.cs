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

        private string CollectionName
        {
            get { return Info.Link.ToString(); }
        }

        public SyndicationItem Post(SyndicationItem item)
        {
            store.Put(CollectionName, item.Id, item);
            return item;
        }

        public SyndicationItem Put(SyndicationItem item)
        {
            return Post(item);
        }

        public void Delete(string id)
        {
            store.Delete(CollectionName, id);
        }

        public IEnumerable<SyndicationItem> List()
        {
            return store.List(CollectionName);
        }

        public SyndicationItem Get(string id)
        {
            return store.Get(CollectionName, id);
        }
    }
}
