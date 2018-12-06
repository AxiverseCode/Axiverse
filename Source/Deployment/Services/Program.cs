using Axiverse.Injection;
using Axiverse.Modules;
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
    [Dependency(typeof(ChatServiceModule))]
    [Dependency(typeof(IdentityServiceModule))]
    [Dependency(typeof(EntityServiceModule))]
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
            Console.WriteLine("Press any key to stop the server...");

            // Start
            server.Start();

            // Shutdown
            Console.CancelKeyPress += (sender, e) => shutdownEvent.Set();
            shutdownEvent.WaitOne();

            // Cleanup
            Console.WriteLine("Shutting down services server.");
            server.ShutdownAsync().Wait();
        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }
    }
}
