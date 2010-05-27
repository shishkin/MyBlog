using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    public class ResourceCollection<T>
        : IResourceCollection
        where T : SyndicationItem
    {
        private readonly Dictionary<string, T> items =
            new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public ResourceCollection(ResourceCollectionInfo info)
        {
            Info = info;
        }

        public ResourceCollectionInfo Info { get; private set; }

        public T Post(T item)
        {
            items.Add(item.Id, item);
            return item;
        }

        public T Put(T item)
        {
            items[item.Id] = item;
            return item;
        }

        public void Delete(string id)
        {
            items.Remove(id);
        }

        public IEnumerable<T> List()
        {
            return items.Values
                .OrderByDescending(x => x.LastUpdatedTime);
        }

        public T Get(string id)
        {
            return items[id];
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
