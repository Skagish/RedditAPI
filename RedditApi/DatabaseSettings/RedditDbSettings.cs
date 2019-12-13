using RedditApi.Models.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditApi.Repositories
{
    public class RedditDbSettings : IRedditDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ThreadsCollectionName { get; set; }
    }
}
