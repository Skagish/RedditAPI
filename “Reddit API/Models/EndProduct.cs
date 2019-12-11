using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Reddit_API.Models
{
    public class EndProduct
    {
        public string Title { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}