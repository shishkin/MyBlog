using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Web;

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
                    Workspaces =
                        {
                            new Workspace
                            {
                                Title = new TextSyndicationContent("Blog"),
                                Collections =
                                    {
                                        new ResourceCollectionInfo
                                        {
                                            Title = new TextSyndicationContent("Posts"),
                                            BaseUri = new Uri("posts", UriKind.Relative),
                                            Accepts = {"application/atom+xml;type=entry"}
                                        },
                                        new ResourceCollectionInfo
                                        {
                                            Title = new TextSyndicationContent("Images"),
                                            BaseUri = new Uri("images", UriKind.Relative),
                                            Accepts = { "image/png", "image/jpeg", "image/gif" }
                                        }
                                    }
                            }
                        }
                });
        }
    }
}