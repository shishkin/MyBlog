using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.ServiceModel.Syndication;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RedBranch.Hammock;
using RedBranch.Hammock.Design;

namespace MyBlog.Web.Data
{
    using Models;

    public class CouchDB : IDataStore
    {
        private readonly Connection connection =
            new CouchProcess(new Uri("http://localhost:5984")).Connect();

        public CouchDB()
        {
            if (connection.ListDatabases().Contains("blog"))
            {
                connection.DeleteDatabase("blog");
                connection.CreateDatabase("blog");
            }
        }

        public void Put(SyndicationItem value)
        {
            value.Id = value.Id.ToSlug();
            InSession(x => x.Save(value));
        }

        public SyndicationItem Get(string id)
        {
            return InSession(x => x.Get(id));
        }

        public void Delete(string id)
        {
            InSession(x => {
                var item = x.Get(id);
                x.Delete(item);
            });
        }

        public IEnumerable<SyndicationItem> List()
        {
            return InSession(repo => {
                return repo.All().ToList();
            });
        }

        private void InSession(Action<Repository<SyndicationItem>> action)
        {
            using (var session = connection.CreateSession("blog"))
            {
                var serializer = session.Serializer.GetJsonSerializer();
                serializer.Converters.Add(new SyndicationContentConverter());

                var repo = new Repository<SyndicationItem>(session);
                action(repo);
            }
        }

        private T InSession<T>(Func<Repository<SyndicationItem>, T> func)
        {
            T value = default(T);
            InSession(x => { value = func(x); });
            return value;
        }
    }
}