using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using System.Net;

namespace MyBlog.Web.AtomPub
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AtomAdapter
    {
        public static ISyndicationService service;
        public static ISyndicationItemFactory itemFactory;

        [OperationContract]
        [WebGet(UriTemplate = "/")]
        public AtomPub10ServiceDocumentFormatter ServiceDocument()
        {
            var doc = service.ServiceDocument();
            doc.BaseUri = GetRequestUri();
            Response(x => x.ContentType = ContentTypes.AtomServiceDocument);
            return new AtomPub10ServiceDocumentFormatter(doc);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{collection}")]
        public Atom10FeedFormatter Feed(string collection)
        {
            var feed = service.Feed(collection);
            feed.BaseUri = GetRequestUri();
            Response(x => x.ContentType = ContentTypes.Atom);
            return feed.GetAtom10Formatter();
        }

        [OperationContract]
        [WebGet(UriTemplate = "{collection}/{id}")]
        public Atom10ItemFormatter Get(string collection, string id)
        {
            var item = service.Get(collection, id);
            if (item == null)
            {
                Response(x => x.StatusCode = HttpStatusCode.NotFound);
                return null;
            }

            item.BaseUri = GetRequestUri();
            Response(x => x.ContentType = ContentTypes.AtomEntry);
            return item.GetAtom10Formatter();
        }

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "{collection}")]
        public Atom10ItemFormatter Post(string collection, Stream stream)
        {
            var item = itemFactory.CreateItem(Request(x => x), stream);
            item.BaseUri = GetRequestUri();
            service.Post(collection, item);
            var itemUri = GetRequestUri().Append(item.Id);
            Response(x => x.ContentType = ContentTypes.AtomEntry);
            Response(x => x.SetStatusAsCreated(itemUri));
            return item.GetAtom10Formatter();
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{collection}/{id}")]
        public Atom10ItemFormatter Put(string collection, string id, Stream stream)
        {
            var item = itemFactory.CreateItem(Request(x => x), stream);
            item.Id = id;
            item.BaseUri = GetRequestUri();
            service.Put(collection, item);
            var itemUri = GetRequestUri().Append(item.Id);
            Response(x => x.ContentType = ContentTypes.AtomEntry);
            Response(x => x.SetStatusAsCreated(itemUri));
            return item.GetAtom10Formatter();
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{collection}/{id}")]
        public void Delete(string collection, string id)
        {
            service.Delete(collection, id);
        }

        private void Response(Action<OutgoingWebResponseContext> setter)
        {
            setter(WebOperationContext.Current.OutgoingResponse);
        }

        private T Request<T>(Func<IncomingWebRequestContext, T> getter)
        {
            return getter(WebOperationContext.Current.IncomingRequest);
        }

        private Uri GetRequestUri()
        {
            return Request(x => x.UriTemplateMatch.RequestUri)
                //.Modify(x => x.Host = "ipv4.fiddler")
                ;
        }
    }
}