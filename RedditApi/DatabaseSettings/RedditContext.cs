using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RedditApi.Models;
using RedditApi.Models.BsonModels;

namespace RedditApi.Repositories
{
    public class RedditContext
    {
        private readonly IMongoDatabase _database = null;

        public  RedditContext(IOptions<RedditDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<ThreadsInBsonWrapper> ThreadsInBson
        {
            get
            {
                return _database.GetCollection<ThreadsInBsonWrapper>("Threads");
            }
        }
    }
}
