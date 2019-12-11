using Newtonsoft.Json;

namespace _Reddit_API.Models
{
    public class ListingComments
    {
        [JsonProperty("kind", Required = Required.Default)]
        public string Kind { get; set; }
        [JsonProperty("data", Required = Required.Always)]
        public ListingCommentData ListingData { get; set; }
    }
}