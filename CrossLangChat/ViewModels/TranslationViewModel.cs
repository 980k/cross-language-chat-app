
namespace CrossLangChat.ViewModels
{
    public class TranslationViewModel
    {
        public string ? Text { get; set; }
        public string ? TargetLanguage { get; set; }
        public List<string> ? TranslatedTexts { get; set; }
    }
}