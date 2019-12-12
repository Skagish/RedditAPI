using Newtonsoft.Json;

namespace Reddit_API.Models.CommentTree
{
    public class Comment
    {
        [JsonProperty("kind", Required = Required.Default)]
        public string Kind { get; set; }
        [JsonProperty("data", Required = Required.Always)]
        public CommentData Data { get; set; }
    }
}
