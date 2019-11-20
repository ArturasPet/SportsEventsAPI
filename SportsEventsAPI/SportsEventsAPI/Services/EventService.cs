using SportsEventsAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace SportsEventsAPI.Services
{
    public class EventService
    {
        private readonly IMongoCollection<Event> _events;
        private readonly IMongoCollection<Sport> _sports;

        public EventService(SportsEventsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _events = database.GetCollection<Event>(settings.EventsCollectionName);
            _sports = database.GetCollection<Sport>(settings.SportsCollectionName);
        }
      
        public List<Event> Get() =>
            _events.Find(Event => true).ToList();

        public Event Get(string id) =>
            _events.Find<Event>(Event => Event.Id == id).FirstOrDefault();

        public List<Event> GetEventsBySport(string sportId)
        {
            return _events.Find(Event => Event.SportId == sportId).ToList();
        }

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

        public void RemoveParticipant(string participantId)
        {
            _events.Find(Event => Event.ParticipantIds.Contains(participantId)).ForEachAsync(EventWithUser =>
            {

                //RoomWithUser.ParticipantIds.Remove(participantId);
                //RemoveAll while there's no control over duplicate users in events
                EventWithUser.ParticipantIds.RemoveAll(id => id == participantId);
                _events.ReplaceOne(_event => _event.Id == EventWithUser.Id, EventWithUser);
            });
        }
    }
}
