using Newtonsoft.Json;

namespace RedditApi.Models.ThreadingTree
{
    public class ThreadData
    {
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }
        [JsonProperty("subreddit", Required = Required.Always)]
        public string Subreddit { get; set; }
        [JsonProperty("body", Required = Required.Default)]
        public string Body { get; set; }
    }
}
