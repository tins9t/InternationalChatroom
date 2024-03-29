using System.Reflection;
using Fleck;
using InternationalChatroom.aiServices;
using InternationalChatroom.Models;
using InternationalChatroom.State;
using lib;

var app = await ApiStartUp.StartApi();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.Run();

public static class ApiStartUp
{
    public static async Task <WebApplication> StartApi()
    {
        var builder = WebApplication.CreateBuilder();

        var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        

        var server = new WebSocketServer("ws://0.0.0.0:8181");
        server.Start(socket =>
        {
            socket.OnOpen = async () =>
            {
                StateService.AddConnection(socket);
                Console.WriteLine("Open!");
                Connections.allSockets.Add(socket);
                await TranslationService.TranslateText("hello", "da");

                try
                {
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException);
                    Console.WriteLine(e.StackTrace);
                }

            };
            socket.OnClose = () =>
            {
                Console.WriteLine("Close!");
                Connections.allSockets.Remove(socket);
            };
            socket.OnMessage = async message =>
            {
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
                try
                {
                    await app.InvokeClientEventHandler(clientEventHandlers, socket, message);

                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException);
                    Console.WriteLine(e.StackTrace);
                    // Write exception here
                }
            };
        });
        return app;
    }
}