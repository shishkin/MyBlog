using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Amazon.SimpleDB.Model;

namespace MyBlog.Web.Data
{
    public static class SimpleDBExtensions
    {
        public static string GetAttribute(this Item item, string name)
        {
            return item.Attribute
                .Where(x => x.Name == name)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static ReplaceableAttribute ToAttribute(this string value, string name)
        {
            return new ReplaceableAttribute()
                .WithName(name)
                .WithValue(value);
        }
    }
}