using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    using Models;

    public class ResourceCollection<T>
        : IResourceCollection
        where T : SyndicationItem
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

        public T Post(T item)
        {
            store.Put(CollectionName, item.Id, item);
            return item;
        }

        public T Put(T item)
        {
            return Post(item);
        }

        public void Delete(string id)
        {
            store.Delete(CollectionName, id);
        }

        public IEnumerable<T> List()
        {
            return store.List<T>(CollectionName);
        }

        public T Get(string id)
        {
            return store.Get<T>(CollectionName, id);
        }

        SyndicationItem IResourceCollection.Get(string id)
        {
            return Get(id);
        }

        IEnumerable<SyndicationItem> IResourceCollection.List()
        {
            return List();
        }

        SyndicationItem IResourceCollection.Post(SyndicationItem item)
        {
            return Post((T)item);
        }

        SyndicationItem IResourceCollection.Put(SyndicationItem item)
        {
            return Put((T)item);
        }
    }
}
