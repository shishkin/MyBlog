using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

namespace MyBlog.Web.Data
{
    using Models;

    public class MongoDB : IDataStore
    {
        public void Put(SyndicationItem value)
        {
            throw new NotImplementedException();
        }

        public SyndicationItem Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SyndicationItem> List()
        {
            throw new NotImplementedException();
        }
    }
}