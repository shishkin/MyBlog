using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.AtomPub
{
    public static class ContentTypes
    {
        public const string Atom = "application/atom+xml";
        public const string AtomEntry = "application/atom+xml;type=entry";
        public const string AtomServiceDocument = "application/atomsvc+xml";
    }
}