using Newtonsoft.Json;
using System.Collections.Generic;

namespace _Reddit_API.Models
{
    public class ListingData
    {
        [JsonProperty("modhash", Required = Required.Default)]
        public string Modhash { get; set; }
        [JsonProperty("dist", Required = Required.Default)]
        public int Dist { get; set; }
        [JsonProperty("children", Required = Required.Always)]
        public ICollection<Thread> Threads { get; set; }
    }
}