using Axiverse.Modules;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Services
{
    public class ServerModule : Module
    {
        protected override void Initialize()
        {
            string portVariable = Environment.GetEnvironmentVariable("AXI_SERVICE_PORT");
            Console.WriteLine("Services port environment variable: {0}", portVariable);
            if (!int.TryParse(portVariable, out var port))
            {
                const int defaultPort = 32000;
                Console.WriteLine("Using default port: {0}", defaultPort);
                port = defaultPort;
            }

            var server = new Server
            {
                Ports = { new ServerPort("0.0.0.0", port, ServerCredentials.Insecure) }
            };

            Bind(server);
        }
    }
}
