using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RedditApi.Models
{
    public class Threads
    {
        [BsonElement("threadsList")]
        public ThreadWrapper threadWrapper { get; set; }

        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
