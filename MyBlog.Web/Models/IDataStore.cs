using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.Models
{
    public interface IDataStore
    {
        void Put(SyndicationItem value);

        SyndicationItem Get(string id);

        void Delete(string id);

        IEnumerable<SyndicationItem> List();
    }
}