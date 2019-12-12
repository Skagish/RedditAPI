using Newtonsoft.Json;
using System.Collections.Generic;

namespace _Reddit_API.Models
{
    public class EndProduct
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public ICollection<Comments> Comments { get; set; }
    }
}