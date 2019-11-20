using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsEventsAPI.Models;
using MongoDB.Driver;


namespace SportsEventsAPI.Services
{
    public class SportService
    {
        private readonly IMongoCollection<Sport> _sports;

        public SportService(SportsEventsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _sports = database.GetCollection<Sport>(settings.SportsCollectionName);
        }

        public List<Sport> Get() =>
            _sports.Find(Sport => true).ToList();

        public Sport Get(string id) =>
            _sports.Find<Sport>(Sport => Sport.Id == id).FirstOrDefault();

        public Sport Create(Sport Sport)
        {
            _sports.InsertOne(Sport);
            return Sport;
        }

        public void Update(string id, Sport SportIn) =>
            _sports.ReplaceOne(Sport => Sport.Id == id, SportIn);

        public void Remove(Sport SportIn) =>
            _sports.DeleteOne(Sport => Sport.Id == SportIn.Id);

        public void Remove(string id) =>
            _sports.DeleteOne(Sport => Sport.Id == id);
    }
}
