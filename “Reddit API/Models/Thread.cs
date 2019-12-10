using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Reddit_API.Models
{
    public class Thread
    {
        public string Thread_id { get; set; }

        public string Title { get; set; }

        public string Subreddit { get; set; }

        public Comments comments { get; set; }

     
    }
}