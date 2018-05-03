using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Grpc.Core;
using Axiverse.Services.Proto;

namespace Axiverse.Services.IdentityService
{
    class Program
    {
        static ManualResetEvent end = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            String port = Environment.GetEnvironmentVariable("IDENTITY_SERVICE_PORT");
            Console.WriteLine("IdentityService reading port " + port);
            if (int.TryParse(port, out var Port)) {
                Port = 32000; 
            }
            Port = 32000;

            Server server = new Server
            {
                Services = { Proto.IdentityService.BindService(new IdentityServiceImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("IdentityService server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");

            Console.CancelKeyPress += (sender, e) => end.Set();
            end.WaitOne();

            Console.WriteLine("Shutting down IdentityService");

            server.ShutdownAsync().Wait();
        }
    }
}
