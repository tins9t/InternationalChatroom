using Fleck;
using InternationalChatroom.Models;
using InternationalChatroom.State;
using lib;
using System.Text.Json;
using InternationalChatroom.aiServices;

namespace InternationalChatroom.ClientEventHandlers;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    public string messageContent { get; set; }
    public int roomId { get; set; }
}

public class ClientWantsToBroadcastToRoom()
    : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    public override async Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        await TranslationService.GetLanguages();
        var insertedMessage = new Message
        {
            message = dto.messageContent,
            username = StateService.Connections[socket.ConnectionInfo.Id].Username
        };
        var message = new ServerBroadcastsMessageWithUsername()
        {
            message = insertedMessage
        };
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(message));
    }
}

public class ServerBroadcastsMessageWithUsername : BaseDto
{
    public Message? message { get; set; }
}