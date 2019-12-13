using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
