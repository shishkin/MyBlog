using System;
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
    }
}