using Newtonsoft.Json;

namespace _Reddit_API.Models
{
    public class Comments
    {
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
    }
}