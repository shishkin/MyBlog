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
    public class MediaService
    {
        private const string title = "Blog - Images";
        private static readonly Dictionary<string, SyndicationItem> items =
            new Dictionary<string, SyndicationItem>();
        private static readonly Dictionary<string, byte[]> blobs =
            new Dictionary<string, byte[]>();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/")]
        public Atom10ItemFormatter Post(Stream stream)
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var item = CreateImageItem(request);
            items.Add(item.Id, item);
            blobs.Add(item.Id, stream.ReadToArray());
            return item.GetAtom10Formatter();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/")]
        public Atom10FeedFormatter List()
        {
            return new SyndicationFeed
            {
                Id = WebOperationContext.Current.IncomingRequest.UriTemplateMatch
                    .RequestUri.AbsoluteUri,
                Title = new TextSyndicationContent(title),
                Items = items.Values.OrderByDescending(x => x.LastUpdatedTime)
            }
            .GetAtom10Formatter();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}")]
        public Stream Get(string id)
        {
            var response = WebOperationContext.Current.OutgoingResponse;
            response.ContentLength = blobs[id].LongLength;
            response.ContentType = items[id].Content.Type;
            return new MemoryStream(blobs[id]);
        }

        private static SyndicationItem CreateImageItem(IncomingWebRequestContext request)
        {
            var slug = request.Headers["Slug"];
            var contentType = request.ContentType;
            var uri = request.UriTemplateMatch.RequestUri.Append(slug);
            return new SyndicationItem()
            {
                Title = new TextSyndicationContent(slug),
                Id = uri.AbsoluteUri,
                Content = new UrlSyndicationContent(uri, contentType),
                Links =
                {
                    new SyndicationLink
                    {
                        Uri = uri,
                        RelationshipType = "enclosure",
                        Title = slug,
                        MediaType = contentType,
                        Length = request.ContentLength
                    }
                }
            };
        }
    }
}