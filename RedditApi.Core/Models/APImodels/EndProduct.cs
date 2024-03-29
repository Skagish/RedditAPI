﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditApi.Models
{
    public class EndProduct
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        
        [JsonProperty(PropertyName = "comments")]
        public List<string> Comments { get; set; }
    }
}
