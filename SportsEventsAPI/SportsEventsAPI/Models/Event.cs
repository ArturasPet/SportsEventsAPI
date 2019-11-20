using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SportsEventsAPI.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string EventName { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public int MaxPlayers { get; set; }
    }
}
