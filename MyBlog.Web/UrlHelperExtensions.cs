using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyBlog.Web
{
    public static class UrlHelperExtensions
    {
        public static string AbsoluteContent(this UrlHelper urlHelper, string contentPath)
        {
            var request = urlHelper.RequestContext.HttpContext.Request;
            var full = request.Url.GetLeftPart(UriPartial.Path);
            var end = request.AppRelativeCurrentExecutionFilePath.TrimStart('~');
            var start = full.EndsWith(end, StringComparison.OrdinalIgnoreCase) ?
                full.Remove(full.Length - end.Length) :
                full;
            return string.Join("/", start.TrimEnd('/'), contentPath.TrimStart('~', '/'));
        }

        public static Uri Append(this Uri baseUri, string relativeUri)
        {
            var uri = new Uri(baseUri.AbsoluteUri.TrimEnd('/') + '/');
            return new Uri(uri, relativeUri);
        }

        public static Uri OneUp(this Uri uri)
        {
            var builder = new UriBuilder(uri);
            var newPath = uri.Segments.Take(uri.Segments.Length - 1);
            builder.Path = string.Join("/", newPath).TrimStart('/');
            return builder.Uri;
        }

        public static Uri Modify(this Uri uri, Action<UriBuilder> modifier)
        {
            var builder = new UriBuilder(uri);
            modifier(builder);
            return builder.Uri;
        }
    }
}