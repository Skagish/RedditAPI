using Newtonsoft.Json;
using System.Collections.Generic;

namespace _Reddit_API.Models
{
    public class ThreadWrapper
    {
        [JsonProperty(PropertyName = "threads")]
        public ICollection<EndProduct> Threads { get; set; }
    }
}