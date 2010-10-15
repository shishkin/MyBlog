using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

using Norm;
using Norm.BSON;
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
        }

        public void Put(SyndicationItem value)
        {
            InMongo(x => x.Save(value));
        }

        public SyndicationItem Get(string id)
        {
            return InMongo(x => x.FindOne(new { Id = id }));
        }

        public void Delete(string id)
        {
            InMongo(x => x.Delete(new { Id = id }));
        }

        public IEnumerable<SyndicationItem> List()
        {
            return InMongo(x => x.Find());
        }

        private T InMongo<T>(Func<IMongoCollection<SyndicationItem>, T> func)
        {
            T value = default(T);
            InMongo(x => { value = func(x); });
            return value;
        }

        private void InMongo(Action<IMongoCollection<SyndicationItem>> func)
        {
            using (var db = Mongo.Create(connectionString))
            {
                func(db.Database.GetCollection<SyndicationItem>("Articles"));
            }
        }
    }

    public class DateTimeOffsetTypeConverter : IBsonTypeConverter
    {
        public object ConvertFromBson(object data)
        {
            return new DateTimeOffset((DateTime)data, TimeSpan.Zero);
        }

        public object ConvertToBson(object data)
        {
            return ((DateTimeOffset)data).UtcDateTime;
        }

        public Type SerializedType
        {
            get { return typeof(DateTime); }
        }
    }

    public class UriTypeConverter : IBsonTypeConverter
    {
        public object ConvertFromBson(object data)
        {
            return new Uri((string)data);
        }

        public object ConvertToBson(object data)
        {
            return ((Uri)data).OriginalString;
        }

        public Type SerializedType
        {
            get { return typeof(string); }
        }
    }

    public class SyndicationContentTypeConverter : IBsonTypeConverter
    {
        public object ConvertFromBson(object data)
        {
            return SyndicationContent.CreatePlaintextContent((string)data);
        }

        public object ConvertToBson(object data)
        {
            return ((SyndicationContent)data).GetText();
        }

        public Type SerializedType
        {
            get { return typeof(string); }
        }
    }

}