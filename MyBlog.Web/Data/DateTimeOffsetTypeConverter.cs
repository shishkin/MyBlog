using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Norm.BSON;

namespace MyBlog.Web.Data
{
    public class DateTimeOffsetTypeConverter : IBsonTypeConverter
    {
        public object ConvertFromBson(object data)
        {
            return new DateTimeOffset((DateTime)data, TimeSpan.Zero);
        }

        public object ConvertToBson(object data)
        {
            return ((DateTimeOffset)data).UtcDateTime;
        }

        public Type SerializedType
        {
            get { return typeof(DateTime); }
        }
    }
}