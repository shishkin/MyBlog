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
            yield return new ResourceCollection<SyndicationItem>(
                store,
                new ResourceCollectionInfo
                {
                    Title = new TextSyndicationContent("Articles"),
                    Link = new Uri("Articles", UriKind.Relative),
                    Accepts = { ContentTypes.AtomEntry }
                });
            yield return new ResourceCollection<MediaSyndicationItem>(
                store,
                new ResourceCollectionInfo
                {
                    Title = new TextSyndicationContent("Media"),
                    Link = new Uri("Media", UriKind.Relative),
                    Accepts = { "image/png", "image/jpeg", "image/gif" }
                });
        }
    }
}