using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Web;

namespace MyBlog.Web.AtomPub
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ArticleService
    {
        private const string title = "Blog";

        [OperationContract]
        [WebGet(UriTemplate = "/")]
        public Atom10FeedFormatter List()
        {
            return new SyndicationFeed
            {
                Id = WebOperationContext.Current.IncomingRequest
                    .UriTemplateMatch.RequestUri.AbsoluteUri,
                Title = new TextSyndicationContent(title),
                Items = { }
            }.GetAtom10Formatter();
        }
    }
}