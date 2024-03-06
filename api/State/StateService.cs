namespace InternationalChatroom.State;
using Fleck;

public class WebSocketMetaData(IWebSocketConnection connection)
{
    public IWebSocketConnection Connection { get; set; } = connection;
    public string Username { get; set; }
}

public static class StateService
{
    public static Dictionary<Guid, WebSocketMetaData> Connections = new();
    public static Dictionary<int, HashSet<Guid>> Rooms = new();

    public static bool AddConnection(IWebSocketConnection socket)
    {
        return Connections.TryAdd(socket.ConnectionInfo.Id, 
            new WebSocketMetaData(socket));
    }

    public static bool AddToRoom(IWebSocketConnection socket, int room)
    {
        if(!Rooms.ContainsKey(room))
            Rooms.Add(room, new HashSet<Guid>());
        return Rooms[room].Add(socket.ConnectionInfo.Id);
    }

    public static void BroadcastToRoom(int room, string message)
    {
        if(Rooms.TryGetValue(room, out var guids))
            foreach (var guid in guids)
            {
                if (Connections.TryGetValue(guid, out var ws))
                    ws.Connection.Send(message);

            }
    }
}