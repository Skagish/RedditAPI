using Newtonsoft.Json;

namespace Reddit_API.Models.ThreadingTree
{
    public class Thread
    {
        [JsonProperty("kind", Required = Required.Default)]
        public string Kind { get; set; }
        [JsonProperty("data", Required = Required.Always)]
        public ThreadData Data { get; set; }
    }
}
