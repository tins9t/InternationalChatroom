using InternationalChatroom.aiServices;
using Microsoft.AspNetCore.Mvc;

namespace InternationalChatroom.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    
    [Route("/api/languages")]
    [HttpGet]
    public Task<Dictionary<string, TranslationService.LanguageInfo>> GetLanguages()
    {
        return TranslationService.GetLanguages();
    }
    
    [Route("/api/translate")]
    [HttpPost]
    public async Task<string> TranslateText([FromBody] TranslationService.MessageToTranslate dto)
    {
        return await TranslationService.TranslateText(dto.Text, dto.To);
    }
}