using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Models
{
    public interface IDataStore
    {
        void Put<T>(string collection, string key, T value);

        T Get<T>(string collection, string key);

        void Delete(string collection, string key);

        IEnumerable<T> List<T>(string collection);
    }
}