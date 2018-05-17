using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Components;
using Axiverse.Simulation.Systems;

namespace Axiverse.Simulation
{
    public class Universe
    {
        public Universe()
        {
            for (int i = 0; i < 5; i++)
            {
                var entity = new Entity();
                entity.Set(new NavigationComponent());
                entities.Add(entity.Identifier, entity);
            }

            systems.Add(new NavigationSystem());
            systems.Add(new SpatialSystem());
        }

        public void Post(Guid target)
        {

        }

        public void Step(float dt)
        {
            //Console.WriteLine("===== Stepping =====");

            foreach (var system in systems)
            {
                foreach (var entity in entities.Values)
                {
                    system.Process(entity, dt);
                }
            }

            foreach (var entity in entities.Values)
            {
                //entity.Model?.Process(entity);
                //Console.WriteLine(entity);
            }
        }

        private readonly Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();
        private readonly List<System> systems = new List<System>();
    }
}
