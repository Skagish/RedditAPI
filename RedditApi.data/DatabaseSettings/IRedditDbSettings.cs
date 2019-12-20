namespace RedditApi.Models.IRepo
{
    public interface IRedditDbSettings
    {
        string ThreadsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string Container { get; set; }
    }
}
