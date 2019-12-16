using RedditApi.Models.IRepo;

namespace RedditApi.Repositories
{
    public class RedditDbSettings : IRedditDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ThreadsCollectionName { get; set; }
    }
}
