using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Services.Proto;
using Axiverse.Simulation;
using Grpc.Core;

namespace Axiverse.Services.EntityService
{
    public class EntityServiceImpl : Proto.EntityService.EntityServiceBase
    {
        Universe universe;
        Runner runner;
        Task running;

        public EntityServiceImpl()
        {
            universe = new Universe();
            runner = new Runner();
            runner.Universe = universe;

            running = runner.Run();
        }

        public override Task Stream(
            IAsyncStreamReader<ClientEvent> requestStream,
            IServerStreamWriter<ServerEvent> responseStream,
            ServerCallContext context)
        {
            return base.Stream(requestStream, responseStream, context);
        }
    }
}
