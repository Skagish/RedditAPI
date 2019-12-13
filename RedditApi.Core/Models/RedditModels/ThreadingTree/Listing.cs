using Newtonsoft.Json;

namespace RedditApi.Models.ThreadingTree
{
    public class Listing
    {
        [JsonProperty("kind", Required = Required.Default)]
        public string Kind { get; set; }
        [JsonProperty("data", Required = Required.Always)]
        public ListingData ListingData { get; set; }
    }
}
