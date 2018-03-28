using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grpc.Core;

namespace Axiverse.Services.IdentityService
{
    class IdentityModule
    {
        public void Install()
        {
            var port = 9090;

            Server server = new Server
            {
                Services = { Proto.IdentityService.BindService(new IdentityServiceImpl()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("RouteGuide server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();

        }
    }
}
