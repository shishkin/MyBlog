using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace MyBlog.Web
{
    public static class HtmlExtensions
    {
        public static HtmlHead AddLink(this HtmlHead head, string href, string rel, string type)
        {
            var link = new HtmlLink
            {
                Href = href
            };
            link.Attributes["rel"] = rel;
            link.Attributes["type"] = type;
            head.Controls.Add(link);
            return head;
        }
    }
}