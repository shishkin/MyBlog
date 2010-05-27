using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.IO;

namespace MyBlog.Web.AtomPub
{
    public class MediaSyndicationItemHandler : ISyndicationItemHandler
    {
        private readonly Regex allowedContentTypes = new Regex(
            "^image/(png|jpeg|gif)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        public bool CanCreateItem(IncomingWebRequestContext request)
        {
            return allowedContentTypes.IsMatch(request.ContentType);
        }

        public SyndicationItem CreateItem(IncomingWebRequestContext request, Stream stream)
        {
            var slug = request.Headers["Slug"];
            var contentType = request.ContentType;
            return new MediaSyndicationItem()
            {
                Title = new TextSyndicationContent(slug),
                Id = slug,
                MediaContentType = contentType,
                MediaContent = stream.ReadToArray(),
                Links =
                {
                    new SyndicationLink
                    {
                        Uri = new Uri(slug, UriKind.Relative),
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