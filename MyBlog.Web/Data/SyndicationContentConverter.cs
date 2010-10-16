using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyBlog.Web.Data
{
    public class SyndicationContentConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(SyndicationContent).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var json = JObject.Load(reader);
            if ((string)json.Property("Type").Value == "html")
            {
                return SyndicationContent.CreateHtmlContent((string)json.Property("Text"));
            }
            else
            {
                return SyndicationContent.CreatePlaintextContent((string)json.Property("Text"));
            }
        }
    }
}