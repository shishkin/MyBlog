using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    using Models;

    public class Blog : SyndicationService
    {
        public Blog(IDataStore store) : base("Blog", GetResourceCollections(store)) { }

        public SyndicationFeed Articles { get { return Feed("Articles"); } }

        private static IEnumerable<IResourceCollection> GetResourceCollections(
            IDataStore store)
        {
            yield return new ResourceCollection(
                store,
                new ResourceCollectionInfo
                {
                    Title = new TextSyndicationContent("Articles"),
                    Link = new Uri("Articles", UriKind.Relative),
                    Accepts = { ContentTypes.AtomEntry }
                });
        }
    }
}