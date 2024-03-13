using InternationalChatroom.aiServices;
using Microsoft.AspNetCore.Mvc;

namespace InternationalChatroom.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    
    [Route("/api/languages")]
    [HttpGet]
    public Task<Dictionary<string, LanguageInfo>> GetLanguages()
    {
        return TranslationService.GetLanguages();
    }
    
    [Route("/api/hello")]
    [HttpGet]
    public string TestConnection()
    {
        return "Hello!";
    }
}