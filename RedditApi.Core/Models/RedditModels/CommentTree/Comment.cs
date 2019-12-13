using Newtonsoft.Json;

namespace RedditApi.Models.CommentTree
{
    public class Comment
    {
        [JsonProperty("kind", Required = Required.Default)]
        public string Kind { get; set; }
        [JsonProperty("data", Required = Required.Always)]
        public CommentData Data { get; set; }
    }
}
