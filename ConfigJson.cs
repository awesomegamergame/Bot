using Newtonsoft.Json;

namespace Discord_Bot
{
    class ConfigJson
    {
        [JsonProperty("t")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}
