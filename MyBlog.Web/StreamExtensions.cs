using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MyBlog.Web
{
    public static class StreamExtensions
    {
        public static byte[] ReadToArray(this Stream stream)
        {
            using (var buffer = new MemoryStream())
            {
                stream.CopyTo(buffer);
                return buffer.ToArray();
            }
        }
    }
}