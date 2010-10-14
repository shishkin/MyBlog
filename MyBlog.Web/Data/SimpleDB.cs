using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Data
{
    using Models;

    public class SimpleDB : IDataStore
    {
        public void Put<T>(string collection, string key, T value)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string collection, string key)
        {
            throw new NotImplementedException();
        }

        public void Delete(string collection, string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List<T>(string collection)
        {
            throw new NotImplementedException();
        }
    }
}