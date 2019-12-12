using Newtonsoft.Json;
using System.Collections.Generic;

namespace Reddit_API.Models.ThreadingTree
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
