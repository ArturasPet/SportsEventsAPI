using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SportsEventsAPI.Models
{
    public class Sport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int MaxTeamSize { get; set; }
    }
}
