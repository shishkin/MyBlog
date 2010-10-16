using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

using Norm.BSON;

namespace MyBlog.Web.Data
{
    public class SyndicationContentTypeConverter : IBsonTypeConverter
    {
        public object ConvertFromBson(object data)
        {
            var obj = data as Expando;
            if (obj == null)
            {
                return null;
            }

            if (obj["Type"] == "html")
            {
                return SyndicationContent.CreateHtmlContent((string)obj["Text"]);
            }
            else
            {
                return SyndicationContent.CreatePlaintextContent((string)obj["Text"]);
            }
        }

        public object ConvertToBson(object data)
        {
            return new Expando(data);
        }

        public Type SerializedType
        {
            get { return typeof(object); }
        }
    }
}