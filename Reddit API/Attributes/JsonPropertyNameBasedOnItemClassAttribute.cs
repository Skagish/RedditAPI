using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_API.Attributes
{
    public class JsonPropertyNameBasedOnItemClassAttribute : Attribute
    {
    }

    public class JsonPluralNameAttribute : Attribute
    {
        public string PluralName { get; set; }
        public JsonPluralNameAttribute(string pluralName)
        {
            PluralName = pluralName;
        }
    }
}
