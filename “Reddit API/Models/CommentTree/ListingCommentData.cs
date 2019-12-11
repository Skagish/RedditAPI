using Newtonsoft.Json;
using System.Collections.Generic;

namespace _Reddit_API.Models
{
    public class ListingCommentData
    {
        [JsonProperty("modhash", Required = Required.Default)]
        public string Modhash { get; set; }
        [JsonProperty("dist", Required = Required.Default)]
        public int Dist { get; set; }
        [JsonProperty("children", Required = Required.Always)]
        public ICollection<Comment> Threads { get; set; }
    }
}