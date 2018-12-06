using Axiverse.Injection;
using Axiverse.Modules;
using Axiverse.Services;
using Axiverse.Services.ChatService;
using Axiverse.Services.EntityService;
using Axiverse.Services.IdentityService;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    //[Dependency(typeof(ChatServiceModule))]
    [Dependency(typeof(IdentityServiceModule))]
    //[Dependency(typeof(EntityServiceModule))]
    //[Dependency(typeof(ServerModule))]
    public class Program : ProgramModule
    {
        private ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        [Bind]
        Server server;

        public override void Execute(string[] args)
        {
            // Setup
            Console.WriteLine("Starting services server.");
            foreach (var item in server.Services)
            {
                Console.WriteLine("\t- {0}", item);

            }
            Console.WriteLine("Press Ctrl-C to stop the server...");

            // Start
            server.Start();

            // Shutdown
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Shutting request recieved.");
                shutdownEvent.Set();
            };
            shutdownEvent.WaitOne();

            // Cleanup
            Console.WriteLine("Shutting down services server.");
            var shutdown = server.ShutdownAsync();
            if (!shutdown.Wait(1000))
            {
                Console.WriteLine("Graceful shutdown timeout, killing server.");
                server.KillAsync().Wait();
            }
            Console.WriteLine("Shutdown complete.");
        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }
    }
}
