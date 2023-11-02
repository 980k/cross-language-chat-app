using Newtonsoft.Json;

namespace CrossLangChat.APIModels
{
    public class TranslationRequest
    {
        [JsonProperty("text")]
        public string[] ? Text { get; set; }

        [JsonProperty("target_lang")]
        public string ? Target_Lang { get; set; }
    }
}
