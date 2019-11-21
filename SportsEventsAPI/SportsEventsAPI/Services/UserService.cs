using System;
using System.Collections.Generic;
using System.Linq;
using SportsEventsAPI.Models;
using MongoDB.Driver;
using SportsEventsAPI.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace SportsEventsAPI.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        List<User> GetAll();
    }
    public class UserService :IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Event> _events;
        private readonly AppSettings _appSettings;

        public UserService(SportsEventsDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
            _events = database.GetCollection<Event>(settings.EventsCollectionName);
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            var user = _users.Find(x => x.UserName == username && x.Password == password).FirstOrDefault();

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public List<User> GetAll() => _users.Find(user => true)
                                            .ToList()
                                            .WithoutPasswords();
        //return _users.WithoutPasswords();

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
