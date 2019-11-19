using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsEventsAPI.Models
{
    public class SportsEventsDatabaseSettings : ISportsEventsDatabaseSettings
    {
        public string SportsCollectionName { get; set; }
        public string EventsCollectionName { get; set; }
        public string ParticipantsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISportsEventsDatabaseSettings
    {
        string SportsCollectionName { get; set; }
        string EventsCollectionName { get; set; }
        string ParticipantsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

