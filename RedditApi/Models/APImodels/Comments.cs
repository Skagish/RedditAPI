using Newtonsoft.Json;

namespace RedditApi.Models
{
    public class Comments
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
