using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Reddit_API.Models
{
    public class Product
    {
        public string Title { get; set; }
        public string Subreddit { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}