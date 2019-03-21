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
using Axiverse.Physics;
using Axiverse.Simulation.Behaviors;
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
            universe.Add(new BehaviorProcessor());

            runner = new Runner();
            runner.Universe = universe;

            running = runner.Run();
            universe.Stepped += (s, e) =>
            {
                var se = new ServerEvent();

                foreach (var entity in universe.Entities)
                {
                    var pc = entity.GetComponent<PhysicsComponent>();
                    var ec = entity.GetComponent<EntityComponent>();
                    se.Entities.Add(new Proto.Entity
                    {
                        Id = entity.Identifier.ToString(),
                        Position = ProtoConverter.Convert(pc.Body.LinearPosition),
                        Velocity = ProtoConverter.Convert(pc.Body.LinearVelocity),
                        Rotation = ProtoConverter.Convert(pc.Body.AngularPosition),
                        Class = ec?.Class ?? ""
                    });
                }

                // Console.WriteLine("Emitting " + se.Entities.Count);


                foreach (var writer in writers)
                {
                    writer.Value.WriteAsync(se);
                }
            };

            for (int i = 0; i < 10; i++)
            {
                var bloid = new SimulationEntity();
                bloid.Components.Add(new TransformComponent
                {
                    Scaling = new Vector3(0.2f, 0.2f, 0.2f)
                });
                bloid.Components.Add(new BehaviorComponent());
                bloid.Components.Add(new PhysicsComponent(new Body
                {
                    LinearPosition = Functions.Random.NextVector3(-10, 10),
                    LinearVelocity = Functions.Random.NextVector3(-10, 10),
                    AngularPosition = Functions.Random.NextQuaternion()
                }));
                bloid.Components.Add(new EntityComponent { Class = "ai" });

                universe.Add(bloid);
            }
        }

        public override async Task Stream(
            IAsyncStreamReader<ClientEvent> requestStream,
            IServerStreamWriter<ServerEvent> responseStream,
            ServerCallContext context)
        {
            var guid = Guid.NewGuid();
            writers.TryAdd(guid, responseStream);
            SimulationEntity entity = null;

            while (await requestStream.MoveNext())
            {
                var current = requestStream.Current;

                var id = Guid.Parse(current.Entity.Id);
                if (!universe.TryGetEntity(id, out entity))
                {
                    entity = new SimulationEntity(id);
                    entity.Components.Add(new EntityComponent());
                    entity.Components.Add(new PhysicsComponent(new Body()));
                    universe.Add(entity);
                }

                var pc = entity.GetComponent<PhysicsComponent>();
                pc.Body.LinearPosition = ProtoConverter.Convert(current.Entity.Position);
                pc.Body.LinearVelocity = ProtoConverter.Convert(current.Entity.Velocity);
                pc.Body.AngularPosition = ProtoConverter.Convert(current.Entity.Rotation);
            }

            if (entity != null)
            {
                universe.Remove(entity);
            }
            writers.TryRemove(guid, out var unused);
        }
    }
}
