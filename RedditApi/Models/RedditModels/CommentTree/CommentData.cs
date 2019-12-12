using Newtonsoft.Json;

namespace RedditApi.Models.CommentTree
{
    public class CommentData
    {
        [JsonProperty("body", Required = Required.Default)]
        public string Body { get; set; }
    }
}
