using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.AtomPub
{
    public class MediaSyndicationItem : SyndicationItem
    {
        public string MediaContentType { get; set; }

        public byte[] MediaContent { get; set; }
    }
}