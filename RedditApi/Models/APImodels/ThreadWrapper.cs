using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditApi.Models
{
    public class ThreadWrapper
    {
        [JsonProperty(PropertyName = "threads")]
        public ICollection<EndProduct> Threads { get; set; }
    }
}
