using NUnit.Framework;
using CrossLangChat.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CrossLangChat.Services
{
    [TestFixture]
    public class DeepLTranslationServiceTest
    {

        [Test]
        public async Task Services_TranslateAsync_ShouldThrowExceptionWhenApiFails()
        {
            string apiKey = "PLACEHOLDER_API_KEY";

            var translationService = new DeepLTranslationService(apiKey);

            var textToTranslate = new string[] { "Hello", "World" };
            var targetLanguage = "ES";

            // Use 'await' before calling the asynchronous method
            Exception? exception = null;
            try
            {
                await translationService.TranslateAsync(textToTranslate, targetLanguage);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.That(exception, Is.Not.Null);
        }
    }
}