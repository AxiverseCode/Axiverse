using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Grpc.Core;
using Axiverse.Modules;

namespace Axiverse.Services.EntityService
{
    public class EntityServiceModule : Module
    {
        static ManualResetEvent end = new ManualResetEvent(false);

        protected override void Initialize()
        {
            string port = Environment.GetEnvironmentVariable("ENTITY_SERVICE_PORT");
            Console.WriteLine("EntityService reading port " + port);
            if (int.TryParse(port, out var Port))
            {
                Port = 32001;
            }
            Port = 32001;

            Server server = new Server
            {
                Services = { Proto.EntityService.BindService(new EntityServiceImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("EntityService server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");

            Console.CancelKeyPress += (sender, e) => end.Set();
            end.WaitOne();

            Console.WriteLine("Shutting down EntityService");

            server.ShutdownAsync().Wait();
        }
    }
}
