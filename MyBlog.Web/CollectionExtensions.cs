using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> CopyTo<T>(this IEnumerable<T> from, ICollection<T> to)
        {
            foreach (var item in from)
            {
                to.Add(item);
            }

            return from;
        }
    }
}