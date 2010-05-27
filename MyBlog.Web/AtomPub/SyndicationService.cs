using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    public class SyndicationService : ISyndicationService
    {
        private readonly Dictionary<string, IResourceCollection> collections;

        public SyndicationService(string title, IEnumerable<IResourceCollection> collections)
        {
            Title = title;
            this.collections = collections.ToDictionary(
                x => x.Info.Title.Text,
                StringComparer.OrdinalIgnoreCase);
        }

        public string Title { get; set; }

        public ServiceDocument ServiceDocument()
        {
            return new ServiceDocument
            {
                Workspaces =
                {
                    new Workspace(Title, collections.Values.Select(x => x.Info))
                }
            };
        }

        public SyndicationFeed Feed(string collectionName)
        {
            var collection = FindCollection(collectionName);

            return new SyndicationFeed
            {
                Id = HttpUtility.UrlEncode(collection.Info.Title.Text),
                Title = collection.Info.Title,
                Items = collection.List()
            };
        }

        public SyndicationItem Get(string collection, string id)
        {
            var atom = FindCollection(collection);
            return atom.Get(id);
        }

        public SyndicationItem Post(string collectionName, SyndicationItem item)
        {
            var collection = FindCollection(collectionName);
            return collection.Post(item);
        }

        public SyndicationItem Put(string collectionName, SyndicationItem item)
        {
            var collection = FindCollection(collectionName);
            return collection.Put(item);
        }

        public void Delete(string collectionName, string id)
        {
            var collection = FindCollection(collectionName);
            collection.Delete(id);
        }

        private IResourceCollection FindCollection(string name)
        {
            return collections[name];
        }
    }
}