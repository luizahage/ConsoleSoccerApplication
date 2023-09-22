using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConsoleSoccerApplication.Services;

namespace ConsoleSoccerApplication.Controllers
{
    public class TranslationController
    {
        private readonly ConfigurationService _configurationService;
        private readonly string subscriptionKey;
        private readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";

        public TranslationController(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
            subscriptionKey = _configurationService.GetAzureTranslatorApiKey();
        }

        public async Task<string> TranslateText(string textToTranslate, string targetLanguage)
        {
            var uri = $"{endpoint}/translate?api-version=3.0&to={targetLanguage}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                var requestBody = new object[] { new { Text = textToTranslate } };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var translationResponse = JsonConvert.DeserializeObject<List<TranslationResponse>>(responseBody);
                    string translatedText = translationResponse.FirstOrDefault()?.Translations.FirstOrDefault()?.Text;

                    return translatedText;
                }
                else
                {
                    return "erro";
                }
            }
        }
    }

    public class TranslationResponse
    {
        public List<Translation> Translations { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string To { get; set; }
    }

}

