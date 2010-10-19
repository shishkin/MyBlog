using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

using Norm;
using Norm.Configuration;
using Norm.Collections;

namespace MyBlog.Web.Data
{
    using Models;

    public class MongoDB : IDataStore
    {
        private const string connectionString = "mongodb://localhost/Blog";

        public MongoDB()
        {
            MongoConfiguration.Initialize(config =>
            {
                config.TypeConverterFor<DateTimeOffset, DateTimeOffsetTypeConverter>();
                config.TypeConverterFor<Uri, UriTypeConverter>();
                config.TypeConverterFor<SyndicationContent, SyndicationContentTypeConverter>();
                config.TypeConverterFor<TextSyndicationContent, SyndicationContentTypeConverter>();
                config.TypeConverterFor<XmlSyndicationContent, SyndicationContentTypeConverter>();
                config.TypeConverterFor<UrlSyndicationContent, SyndicationContentTypeConverter>();
                config.For<SyndicationItem>(map =>
                {
                    map.IdIs(x => x.Id);
                });
            });
            InMongo(items => items.Delete(new { }));
        }

        public void Put(SyndicationItem value)
        {
            InMongo(x => x.Save(value));
        }

        public SyndicationItem Get(string id)
        {
            return InMongo(items => items.FindOne(new { Id = id }));
        }

        public void Delete(string id)
        {
            InMongo(items => items.Delete(new { Id = id }));
        }

        public IEnumerable<SyndicationItem> List()
        {
            return InMongo(items => items.Find());
        }

        private T InMongo<T>(Func<IMongoCollection<SyndicationItem>, T> func)
        {
            T value = default(T);
            InMongo(x => { value = func(x); });
            return value;
        }

        private void InMongo(Action<IMongoCollection<SyndicationItem>> func)
        {
            using (var mongo = Mongo.Create(connectionString))
            {
                func(mongo.Database.GetCollection<SyndicationItem>("Articles"));
            }
        }
    }
}