using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditApi.Models.ThreadingTree
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
