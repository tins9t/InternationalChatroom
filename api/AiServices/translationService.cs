using System.Text;
using Newtonsoft.Json;




namespace InternationalChatroom.aiServices;

public class TranslationService {
    
    public static async Task GetLanguages()
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
                    JsonConvert.DeserializeObject<LanguageRootObject>(rep);
                    Console.WriteLine(LanguageRootObject.languageList);
                    
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
    }
    
    //https://learn.microsoft.com/en-us/azure/ai-services/translator/reference/v3-0-translate
    


    private readonly string Key = Environment.GetEnvironmentVariable("apikey");
    private readonly string Endpoint = "https://chatroomtranslator.cognitiveservices.azure.com/";

    // location, also known as region.
    // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
    private static readonly string Location = "northeurope";

    async Task TranslateText(MessageToTranslate messageToTranslate)
    {
        // Output languages are defined as parameters, input language detected.
        string route = "/translate?api-version=3.0&to=en";
        string textToTranslate = messageToTranslate.textToTranslate;
        object[] body = new object[] { new { Text = textToTranslate } };
        var requestBody = JsonConvert.SerializeObject(body);

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

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
    }
}

public class MessageToTranslate
{
    public string? textToTranslate { get; set; }
    public string? endLanguage { get; set; }
}

public class ResponseModelTranslatedLanguage {
    
    public string? language { get; set; }
    public int score { get; set; }
    public string? text { get; set; }
    public string? to { get; set; }
}
public class DetectedLanguage
{
    public string language { get; set; }
    public double score { get; set; }
}
public class LanguageRootObject
{
    public static Dictionary<string, LanguageInfo> languageList { get; set; }
}
public class LanguageInfo
{
    public string name { get; set; }
    public string nativeName { get; set; }
    public string dir { get; set; }
}

public class Translation
{
    public string text { get; set; }
    public string to { get; set; }
}
