using Newtonsoft.Json;

namespace Reddit_API.Models
{
    public class Comments
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
