using SportsEventsAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace SportsEventsAPI.Services
{
    public class EventService
    {
        private readonly IMongoCollection<Event> _events;

        public EventService(SportsEventsDatabaseSettings settings)
        {
            if (settings != null)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _events = database.GetCollection<Event>(settings.EventsCollectionName);
            }

        }

        public List<Event> Get() =>
            _events.Find(Event => true).ToList();

        public Event Get(string id) =>
            _events.Find<Event>(Event => Event.Id == id).FirstOrDefault();

        public Event Create(Event Event)
        {
            _events.InsertOne(Event);
            return Event;
        }

        public void Update(string id, Event EventIn) =>
            _events.ReplaceOne(Event => Event.Id == id, EventIn);

        public void Remove(Event EventIn) =>
            _events.DeleteOne(Event => Event.Id == EventIn.Id);

        public void Remove(string id) =>
            _events.DeleteOne(Event => Event.Id == id);
    }
}
