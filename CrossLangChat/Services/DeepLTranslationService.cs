using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CrossLangChat.APIModels;
using Newtonsoft.Json;

namespace CrossLangChat.Services
{
    public class DeepLTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public DeepLTranslationService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<List<string>> TranslateAsync(string[] text, string targetLanguage)
        {

            var requestBody = new TranslationRequest
            {
                Text = text,
                Target_Lang = targetLanguage
            };

            var requestJson = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {_apiKey}");

            var response = await _httpClient.PostAsync("https://api-free.deepl.com/v2/translate", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();

                var translationResponse = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);

                var translatedTexts = translationResponse?.Translations
                    ?.Where(t => t?.Text != null)
                    ?.Select(t => t?.Text!)
                    ?.ToList()
                    ?? new List<string>();

                return translatedTexts;
            }
            
            throw new Exception($"Failed to translate text. Status code: {response.StatusCode}");
        }
    }
}