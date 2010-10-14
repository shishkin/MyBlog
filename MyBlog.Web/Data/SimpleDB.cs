using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;

namespace MyBlog.Web.Data
{
    using Models;

    public class SimpleDB : IDataStore
    {
        private readonly AmazonSimpleDB client;
        private const string Domain = "Articles";
        private const string XmlAttribute = "ArticleAsXml";
        private static readonly string SelectExpression =
            string.Format("select {0} from {1}", XmlAttribute, Domain);

        public SimpleDB()
        {
            var appSettings = ConfigurationManager.AppSettings;
            client = AWSClientFactory.CreateAmazonSimpleDBClient(
                appSettings["AWSAccessKey"],
                appSettings["AWSSecretKey"]);
        }

        public void Put(SyndicationItem value)
        {
            client
                .PutAttributes(new PutAttributesRequest()
                .WithDomainName(Domain)
                .WithItemName(value.Id)
                .WithAttribute(GetAttributes(value).ToArray()));
        }

        public SyndicationItem Get(string id)
        {
            return client
                .GetAttributes(new GetAttributesRequest()
                .WithDomainName(Domain)
                .WithItemName(id)
                .WithAttributeName(XmlAttribute)
                .WithConsistentRead(true))
                .GetAttributesResult
                .Attribute
                .Select(x => x.Value.LoadSyndicationItem())
                .SingleOrDefault();
        }

        public void Delete(string id)
        {
            client.DeleteAttributes(new DeleteAttributesRequest()
                .WithDomainName(Domain)
                .WithItemName(id));
        }

        public IEnumerable<SyndicationItem> List()
        {
            var result = client
                .Select(new SelectRequest()
                .WithSelectExpression(SelectExpression));
            return result
                .SelectResult
                .Item
                .Select(ToSyndicationItem);
        }

        private IEnumerable<ReplaceableAttribute> GetAttributes(SyndicationItem value)
        {
            yield return value
                .SaveAsString()
                .ToAttribute(XmlAttribute);
        }

        private static SyndicationItem ToSyndicationItem(Item item)
        {
            return item.GetAttribute(XmlAttribute).LoadSyndicationItem();
        }
    }
}