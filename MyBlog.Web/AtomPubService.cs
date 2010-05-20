using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MyBlog.Web
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AtomPubService
    {
        private static readonly Dictionary<string, SyndicationItem> images = new Dictionary<string, SyndicationItem>();
        private static readonly Dictionary<string, byte[]> imageBlobs = new Dictionary<string, byte[]>();

        [OperationContract]
        [WebGet(UriTemplate = "")]
        public AtomPub10ServiceDocumentFormatter GetServiceDoc()
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var response = WebOperationContext.Current.OutgoingResponse;
            response.ContentType = "application/atomsvc+xml";

            return new AtomPub10ServiceDocumentFormatter(
                new ServiceDocument
                {
                    //BaseUri = request.UriTemplateMatch.RequestUri,
                    BaseUri = new Uri("http://ipv4.fiddler:11147/atom/service/", UriKind.Absolute),
                    Workspaces = { CreateWorkspace("Blog") }
                });
        }

        [OperationContract]
        [WebGet(UriTemplate = "/Posts")]
        public Atom10FeedFormatter GetPosts()
        {
            return new SyndicationFeed
                {
                    Title = new TextSyndicationContent("Blog"),
                    Items = { }
                }
                .GetAtom10Formatter();
        }

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Images")]
        public Atom10ItemFormatter PostImages(Stream stream)
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var item = CreateImageItem(request);
            images.Add(item.Id, item);
            imageBlobs.Add(item.Id, stream.ReadToArray());
            return item.GetAtom10Formatter();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/Images")]
        public Atom10FeedFormatter GetImages()
        {
            return new SyndicationFeed
            {
                Title = new TextSyndicationContent("Blog - Images"),
                Items = images.Values.OrderByDescending(x => x.LastUpdatedTime)
            }
            .GetAtom10Formatter();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/Images/{id}")]
        public Stream GetImage(string id)
        {
            var response = WebOperationContext.Current.OutgoingResponse;
            response.ContentLength = imageBlobs[id].LongLength;
            response.ContentType = images[id].Content.Type;
            return new MemoryStream(imageBlobs[id]);
        }

        private static SyndicationItem CreateImageItem(IncomingWebRequestContext request)
        {
            var slug = request.Headers["Slug"];
            var contentType = request.ContentType;
            var uri = request.UriTemplateMatch.RequestUri.Append(slug);
            return new SyndicationItem()
            {
                Title = new TextSyndicationContent(slug),
                BaseUri = request.UriTemplateMatch.RequestUri,
                Id = slug,
                Content = new UrlSyndicationContent(uri, contentType),
                Links = { new SyndicationLink(uri) }
            };
        }

        private static Workspace CreateWorkspace(string title)
        {
            return new Workspace
            {
                Title = new TextSyndicationContent(title),
                Collections =
                {
                    CreateCollectionInfo("Posts", "application/atom+xml;type=entry"),
                    CreateCollectionInfo("Images", "image/png", "image/jpeg", "image/gif")
                }
            };
        }

        private static ResourceCollectionInfo CreateCollectionInfo(string name, params string[] accepts)
        {
            var info = new ResourceCollectionInfo
            {
                Title = new TextSyndicationContent(name),
                Link = new Uri(HttpUtility.UrlEncode(name), UriKind.Relative),
            };

            accepts.CopyTo(info.Accepts);
            return info;
        }
    }
}