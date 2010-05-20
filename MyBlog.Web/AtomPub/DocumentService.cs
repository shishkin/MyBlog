using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Web;

namespace MyBlog.Web.AtomPub
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class DocumentService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        public AtomPub10ServiceDocumentFormatter Get()
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var response = WebOperationContext.Current.OutgoingResponse;
            response.ContentType = "application/atomsvc+xml";
            var baseUri = request.UriTemplateMatch.RequestUri
                .Modify(x => x.Host = "ipv4.fiddler")
                .OneUp();

            return new AtomPub10ServiceDocumentFormatter(
                new ServiceDocument
                {
                    BaseUri = baseUri,
                    Workspaces = { CreateWorkspace("Blog") }
                });
        }

        private static Workspace CreateWorkspace(string title)
        {
            return new Workspace
            {
                Title = new TextSyndicationContent(title),
                Collections =
                {
                    CreateCollectionInfo("Articles", "application/atom+xml;type=entry"),
                    CreateCollectionInfo("Media", "image/png", "image/jpeg", "image/gif")
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