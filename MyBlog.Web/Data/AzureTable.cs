using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace MyBlog.Web.Data
{
    using Models;

    public class AzureTable : IDataStore
    {
        private const string SetName = "Blog";
        private readonly TableServiceContext context;

        public AzureTable()
        {
            var settings = ConfigurationManager.AppSettings;
            context = new TableServiceContext(
                settings["AzureUrl"],
                new StorageCredentialsAccountAndKey(
                    settings["AzureAccount"],
                    settings["AzureKey"]));
            var client = new CloudTableClient(
                context.BaseUri.OriginalString,
                context.StorageCredentials);
            client.CreateTableIfNotExist(SetName);
        }

        public void Put(SyndicationItem value)
        {
            context.AddObject(
                SetName,
                new SyndicationEntity(value)
                {
                    PartitionKey = SetName,
                    RowKey = value.Id
                });
            context.SaveChanges();
        }

        public SyndicationItem Get(string id)
        {
            return GetEntity(id)
                .Select(x => x.ItemAsXml.LoadSyndicationItem())
                .FirstOrDefault();
        }

        public void Delete(string id)
        {
            GetEntity(id).Do(entity =>
                context.DeleteObject(entity));
        }

        public IEnumerable<SyndicationItem> List()
        {
            return context
                .CreateQuery<SyndicationEntity>(SetName)
                .ToList()
                .Select(x => x.ItemAsXml.LoadSyndicationItem());
        }

        private IEnumerable<SyndicationEntity> GetEntity(string id)
        {
            return context
                .CreateQuery<SyndicationEntity>(SetName)
                .Where(x => x.RowKey == id && x.PartitionKey == SetName)
                .ToList();
        }
    }

    public class SyndicationEntity : TableServiceEntity
    {
        public SyndicationEntity(){}

        public SyndicationEntity(SyndicationItem item)
        {
            Title = item.Title.Text;
            ItemAsXml = item.SaveAsString();
        }

        public string Title { get; set; }

        public string ItemAsXml { get; set; }
    }
}