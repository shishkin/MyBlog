using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Norm.BSON;

namespace MyBlog.Web.Data
{
    public class UriTypeConverter : IBsonTypeConverter
    {
        public object ConvertFromBson(object data)
        {
            return new Uri((string)data);
        }

        public object ConvertToBson(object data)
        {
            return ((Uri)data).OriginalString;
        }

        public Type SerializedType
        {
            get { return typeof(string); }
        }
    }

}