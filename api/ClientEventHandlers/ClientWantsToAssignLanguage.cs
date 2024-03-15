using System.Text.Json;
using Fleck;
using FleckTest;
using InternationalChatroom.Models;
using InternationalChatroom.State;
using lib;


namespace InternationalChatroom.ClientEventHandlers;

public class ClientWantsToAssignLanguageDto : BaseDto
{
    public string translation{ get; set; }

}

public class ClientWantsToAssignLanguage : BaseEventHandler<ClientWantsToAssignLanguageDto>
{

    public override Task Handle(ClientWantsToAssignLanguageDto dto, IWebSocketConnection socket)
    {
        StateService.Connections[socket.ConnectionInfo.Id].Language = dto.translation;
        socket.Send(JsonSerializer.Serialize(new ServerResponse()
        {
            eventType = "ServerResponseWithLanguage",
            message = "Welcome   " + dto.translation
        }));
        return Task.CompletedTask;
    }
}

public class ServerResponse : BaseDto
{
    public string eventType { get; set; }
    public string message { get; set; }
}
    
