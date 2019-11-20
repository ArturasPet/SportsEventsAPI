using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsEventsAPI.Models;
using MongoDB.Driver;

namespace SportsEventsAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Event> _events;

        public UserService(SportsEventsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
            _events = database.GetCollection<Event>(settings.EventsCollectionName);
        }

        public List<User> Get() =>
            _users.Find(User => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(User => User.Id == id).FirstOrDefault();

        public User GetByUsername(string Username) => _users.Find(User => User.UserName == Username).FirstOrDefault();
        public List<User> GetUsersByEvent(string eventId)
        {
            var @event = _events.Find(@event => @event.Id == eventId).FirstOrDefault();
            List<string> participantIds = @event == null ? new List<string>() : @event.ParticipantIds;
            return _users.Find(User => participantIds.Contains(User.Id)).ToList();
        }
        public User Create(User User)
        {
            _users.InsertOne(User);
            return User;
        }

        public void Update(string id, User UserIn) =>
            _users.ReplaceOne(User => User.Id == id, UserIn);

        public void Remove(User UserIn) =>
            _users.DeleteOne(User => User.Id == UserIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(User => User.Id == id);
    }
}
