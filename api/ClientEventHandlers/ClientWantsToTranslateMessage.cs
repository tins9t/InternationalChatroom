using System.Text.Json;
using Fleck;
using InternationalChatroom.aiServices;
using InternationalChatroom.Models;
using InternationalChatroom.State;
using lib;

namespace InternationalChatroom.ClientEventHandlers;

public class ClientWantsToTranslateMessageDto: BaseDto
{
    public string? Text { get; set; }
    public string? To { get; set; }
}

public class ClientWantsToTranslateMessage() : BaseEventHandler<ClientWantsToTranslateMessageDto>
{
    public override async Task Handle(ClientWantsToTranslateMessageDto dto, IWebSocketConnection socket)
    {
         String response = await TranslationService.TranslateText(dto.Text, dto.To);
         Console.WriteLine(response);
         var translationRoot = JsonSerializer.Deserialize<List<TranslationRoot>>(response);

         var detectedLanguage = translationRoot[0].detectedLanguage;
         var translations = translationRoot[0].translations;

         Console.WriteLine($"Detected Language: {detectedLanguage.language}");
         Console.WriteLine($"Score: {detectedLanguage.score}");
         Console.WriteLine($"Translated Text: {translations[0].text}");
         Console.WriteLine($"Target Language: {translations[0].to}");
    }
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class DetectedLanguage
{
    public string language { get; set; }

    public double score { get; set; }
}

public class TranslationRoot
{
    public DetectedLanguage detectedLanguage { get; set; }

    public List<Translation> translations { get; set; }
}

public class Translation
{
    public string text { get; set; }

    public string to { get; set; }
}