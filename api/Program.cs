using System.Reflection;
using Fleck;
using InternationalChatroom.aiServices;
using InternationalChatroom.Models;
using InternationalChatroom.State;
using lib;


public static class Startup
{
    public static void Main(string[] args)
    {
        Statup(args);
        Console.ReadLine();
    }

    public static WebApplication Statup(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        var app = builder.Build();
        

        var server = new WebSocketServer("ws://0.0.0.0:8181");
        server.Start(socket =>
        {
            socket.OnOpen = async () =>
            {
                StateService.AddConnection(socket);
                Console.WriteLine("Open!");
                Connections.allSockets.Add(socket);

                try
                {
                    MessageToTranslate messageToTranslate = new MessageToTranslate();
                    messageToTranslate.To = "de";
                    messageToTranslate.Text = "Hello";
                    await TranslationService.TranslateText(messageToTranslate);
                    await TranslationService.GetLanguages();
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
        app.MapControllers();
        return app;
    }
}