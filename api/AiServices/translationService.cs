using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace InternationalChatroom.aiServices;

public class TranslationService
{

    public static async Task<Dictionary<string, LanguageInfo>> GetLanguages()
    {

        //https://learn.microsoft.com/en-us/azure/ai-services/translator/reference/v3-0-languages
        try
        {
            string url = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                var rep = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LanguageRootObject languageRootObject = JsonSerializer.Deserialize<LanguageRootObject>(rep);
                    foreach (var language in languageRootObject.translation)
                    {
                        Console.WriteLine($"Language Code: {language.Key}");
                        Console.WriteLine($"Name: {language.Value.name}");
                        Console.WriteLine($"Native Name: {language.Value.nativeName}");
                        Console.WriteLine($"Dir: {language.Value.dir}");
                        Console.WriteLine();
                    }

                    return languageRootObject.translation;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            Console.WriteLine(ex.InnerException);
            Console.WriteLine(ex.StackTrace);
        }

        throw new Exception();
    }

    //https://learn.microsoft.com/en-us/azure/ai-services/translator/reference/v3-0-translate





    public static async Task<string> TranslateText(string text, string to)
    {
        {
            string Key = Environment.GetEnvironmentVariable("apikey");
            string Endpoint = "https://api.cognitive.microsofttranslator.com";

            // location, also known as region.
            // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
            string Location = "northeurope";
            // Output languages are defined as parameters, input language detected.

            string route = "/translate?api-version=3.0" + "&to=" + to;
            string textToTranslate = text;
            object[] body = new object[]
            {
                new
                {
                    Text = textToTranslate
                }
            };
            var requestBody = JsonSerializer.Serialize(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(Endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", Key);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add("Ocp-Apim-Subscription-Region", Location);
                Console.WriteLine(request);
                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                return result;

            }
        }
    }

    public class MessageToTranslate
    {
        public string? Text { get; set; }

        public string? To { get; set; }
    }



    public class LanguageRootObject
    {
        public Dictionary<string, LanguageInfo> translation { get; set; }
    }

    public class LanguageInfo
    {
        public string name { get; set; }

        public string nativeName { get; set; }

        public string dir { get; set; }
    }
}


