namespace InternationalChatroom.Models;

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


