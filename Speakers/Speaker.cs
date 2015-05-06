using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;


namespace AspNet5MongoDb.Speakers
{
    public class Speaker
    {
        [BsonId]
        [JsonIgnore]
        [XmlIgnore]
        public ObjectId MongoId { get; private set; }

        public string Id
        {
            get { return MongoId.ToString(); }
            set { MongoId = ObjectId.Parse(value); }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
