using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Web
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AtomPubService
    {
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
                    BaseUri = request.UriTemplateMatch.RequestUri,
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
                BaseUri = new Uri(HttpUtility.UrlEncode(name), UriKind.Relative),
            };

            accepts.CopyTo(info.Accepts);
            return info;
        }
    }
}