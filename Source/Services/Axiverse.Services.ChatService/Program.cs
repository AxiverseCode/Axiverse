using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Grpc.Core;

namespace Axiverse.Services.ChatService
{
    class Program
    {
        static ManualResetEvent end = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            String port = Environment.GetEnvironmentVariable("CHAT_SERVICE_PORT");
            Console.WriteLine("ChatService reading port " + port);
            if (int.TryParse(port, out var Port))
            {
                Port = 32002;
            }
            if (Port == 0) Port = 32002;

            Server server = new Server
            {
                Services = { Proto.ChatService.BindService(new ChatServiceImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
                //Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("ChatService server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");

            Console.CancelKeyPress += (sender, e) => end.Set();
            end.WaitOne();

            Console.WriteLine("Shutting down ChatService");

            server.ShutdownAsync().Wait();
        }
    }
}
