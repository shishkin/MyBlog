using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.Models
{
    public interface IDataStore
    {
        void Put(string collection, string key, SyndicationItem value);

        SyndicationItem Get(string collection, string key);

        void Delete(string collection, string key);

        IEnumerable<SyndicationItem> List(string collection);
    }
}