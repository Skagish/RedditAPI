using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RedditApi.Models.BsonModels
{
    public class ThreadsInBson
    {
        [BsonElement( "title")]
        public string Title { get; set; }

        [BsonElement( "comments")]
        public List<string> Comments { get; set; }
    }
}
