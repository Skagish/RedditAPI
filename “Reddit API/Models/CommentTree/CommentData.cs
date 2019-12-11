using Newtonsoft.Json;

namespace _Reddit_API.Models
{
    public class CommentData
    {
        [JsonProperty("body", Required = Required.Default)]
        public string Body { get; set; }
    }
}