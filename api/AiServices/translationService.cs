using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    await GetListofLanguages(content);
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
        }
    }
    
    //https://learn.microsoft.com/en-us/azure/ai-services/translator/reference/v3-0-translate
    static Task GetListofLanguages(String responseContent)
    {
        // Parse the JSON response
        JObject parsedResponse = JObject.Parse(responseContent);

        // Extract language names and codes
        List<string> languageList = new List<string>();
        foreach (var lang in parsedResponse["translation"])
        {
            string langCode = lang.Path;
            string langName = lang["name"].ToString();
            languageList.Add($"{langName} ({langCode})");
        }

        // Print the language list
        Console.WriteLine("Language List:");
        foreach (var item in languageList)
        {
            Console.WriteLine(item);
        }
        return Task.CompletedTask;
    }
    
    


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

public class TranslatedLanguage {
    
    public string? language { get; set; }
    public int score { get; set; }
    public string? text { get; set; }
    public string? to { get; set; }
        
   
}
