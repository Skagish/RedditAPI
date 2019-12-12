using Newtonsoft.Json;

namespace Reddit_API.Models.CommentTree
{
    public class CommentData
    {
        [JsonProperty("body", Required = Required.Default)]
        public string Body { get; set; }
    }
}
