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

        //[BsonElement("Title")]
        public string Name { get; set; }
        public string City { get; set; }
        public string SportId { get; set; }
        public List<string> ParticipantIds { get; set; }
    }
}