
namespace CrossLangChat.APIModels
{
    public class TranslationResponse
    {
         public List<Translation> ? Translations { get; set; }
    }

    public class Translation
    {
        public string ? DetectedSourceLanguage { get; set; }
        public string ? Text { get; set; }
    }
}