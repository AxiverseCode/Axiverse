using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Services.Proto;
using Axiverse.Simulation;
using Grpc.Core;

using System.Collections.Concurrent;
using SimulationEntity = Axiverse.Simulation.Entity;
using Axiverse.Interface.Scenes;

namespace Axiverse.Services.EntityService
{
    public class EntityServiceImpl : Proto.EntityService.EntityServiceBase
    {
        Universe universe;
        Runner runner;
        Task running;
        ConcurrentDictionary<Guid, IServerStreamWriter<ServerEvent>> writers = new ConcurrentDictionary<Guid, IServerStreamWriter<ServerEvent>>();


        public EntityServiceImpl()
        {
            universe = new Universe();
            runner = new Runner();
            runner.Universe = universe;

            running = runner.Run();
            universe.Stepped += (s, e) =>
            {
                var se = new ServerEvent();

                foreach (var entity in universe.Entities)
                {
                    var tc = entity.GetComponent<TransformComponent>();
                    se.Entities.Add(new Proto.Entity
                    {
                        Id = entity.Identifier.ToString(),
                        Position = ProtoConverter.Convert(tc.Translation),
                        Rotation = ProtoConverter.Convert(tc.Rotation),
                    });
                }

                Console.WriteLine("Emitting " + se.Entities.Count);


                foreach (var writer in writers)
                {
                    writer.Value.WriteAsync(se);
                }
            };
        }

        public override async Task Stream(
            IAsyncStreamReader<ClientEvent> requestStream,
            IServerStreamWriter<ServerEvent> responseStream,
            ServerCallContext context)
        {
            var guid = Guid.NewGuid();
            writers.TryAdd(guid, responseStream);

            while (await requestStream.MoveNext())
            {
                var current = requestStream.Current;

                var id = Guid.Parse(current.Entity.Id);
                if (!universe.TryGetEntity(id, out var entity))
                {
                    entity = new SimulationEntity(id);
                    entity.Components.Add(new TransformComponent());
                    universe.Add(entity);
                }

                entity.GetComponent<TransformComponent>().Translation = ProtoConverter.Convert(current.Entity.Position);
                entity.GetComponent<TransformComponent>().Rotation = ProtoConverter.Convert(current.Entity.Rotation);
            }

            writers.TryRemove(guid, out var unused);
        }
    }
}
