using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace RedditApi.Models
{
    public class Threads
    {
        [JsonProperty(PropertyName = "threadsList")]
        public ThreadWrapper ThreadWrapper { get; set; }

        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
