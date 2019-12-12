using System.Collections.Generic;

namespace RedditApi.Models
{
    public class Product
    {
        public string Title { get; set; }
        public string Subreddit { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
